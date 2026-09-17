using System;
using UnityEngine;
#if !HEADLESS
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Vosk;
#endif

namespace RogueAi.Voice
{
    /// <summary>
    /// Windows x64 speech provider backed by the offline Vosk recogniser.
    ///
    /// Pipeline: Microphone.Start() captures 16 kHz mono audio into a looping AudioClip;
    /// a background thread polls new samples in 4096-sample chunks, converts them to 16-bit
    /// PCM and feeds them to a Vosk recognizer. When the recognizer reports a completed
    /// utterance we parse its JSON, normalise the text, measure RMS amplitude, classify the
    /// cast volume, and marshal a <see cref="VoiceRecognitionResult"/> back to the main
    /// thread where <see cref="OnPhraseRecognized"/> fires.
    ///
    /// All Vosk / Microphone usage is wrapped in <c>#if !HEADLESS</c> so dedicated-server /
    /// CI builds (which define HEADLESS) compile without the native binary and simply stay
    /// silent.
    /// </summary>
    [System.Serializable]
    public class VoiceRecognizerJson // JsonUtility target for Vosk's {"text":"..."} payload
    {
        public string text;
        public string partial;
    }

    public class VoskVoiceInputService : IVoiceInputService
    {
        public bool IsListening { get; private set; }
        public event Action<VoiceRecognitionResult> OnPhraseRecognized;

        // Model lives under StreamingAssets so it ships read-only with the player.
        public const string ModelRelativePath = "VoskModels/small-en-us";
        private const int SampleRate = 16000;
        private const int ChunkSize = 4096;
        private const int MicLoopSeconds = 1;

#if !HEADLESS
        private Model _model;
        private VoskRecognizer _recognizer;
        private Thread _worker;
        private volatile bool _running;
        private AudioClip _micClip;
        private string _micDevice;

        private readonly ConcurrentQueue<VoiceRecognitionResult> _pending =
            new ConcurrentQueue<VoiceRecognitionResult>();
        private MainThreadPump _pump;
#endif

        public void StartListening()
        {
            if (IsListening)
                return;

#if HEADLESS
            Debug.LogWarning("[Vosk] Voice input disabled in HEADLESS build.");
            return;
#else
            if (Microphone.devices == null || Microphone.devices.Length == 0)
            {
                // Directive: must not crash without a mic — log and stay silent.
                Debug.LogWarning("[Vosk] No microphone device found; voice casting will stay silent.");
                return;
            }

            if (!TryLoadModel())
                return;

            try
            {
                _recognizer = new VoskRecognizer(_model, SampleRate);
                _recognizer.SetWords(true);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Vosk] Failed to create recognizer: {e.Message}");
                DisposeVosk();
                return;
            }

            _micDevice = Microphone.devices[0];
            _micClip = Microphone.Start(_micDevice, true, MicLoopSeconds, SampleRate);
            if (_micClip == null)
            {
                Debug.LogWarning("[Vosk] Microphone.Start returned null; voice casting will stay silent.");
                DisposeVosk();
                return;
            }

            EnsurePump();
            IsListening = true;
            _running = true;
            _worker = new Thread(CaptureLoop) { IsBackground = true, Name = "VoskCaptureLoop" };
            _worker.Start();
            Debug.Log($"[Vosk] Listening on '{_micDevice}' @ {SampleRate}Hz.");
#endif
        }

        public void StopListening()
        {
            if (!IsListening)
                return;
            IsListening = false;

#if !HEADLESS
            _running = false;
            try { _worker?.Join(500); } catch { /* ignore */ }
            _worker = null;

            // Flush any final utterance the recognizer was still buffering.
            try
            {
                if (_recognizer != null)
                {
                    string finalJson = _recognizer.FinalResult();
                    EmitFromJson(finalJson, lastRms: 0f);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Vosk] FinalResult failed: {e.Message}");
            }

            if (!string.IsNullOrEmpty(_micDevice) && Microphone.IsRecording(_micDevice))
                Microphone.End(_micDevice);
            _micClip = null;

            DisposeVosk();
            Debug.Log("[Vosk] Stopped listening.");
#endif
        }

#if !HEADLESS
        private bool TryLoadModel()
        {
            string modelPath = Path.Combine(Application.streamingAssetsPath, ModelRelativePath);
            if (!Directory.Exists(modelPath))
            {
                Debug.LogWarning(
                    $"[Vosk] Model not found at '{modelPath}'. Run " +
                    "'Plunderspell/Voice/Download Vosk Small Model' from the editor menu. " +
                    "Voice casting will stay silent.");
                return false;
            }

            try
            {
                Vosk.Vosk.SetLogLevel(-1); // silence native chatter
                _model = new Model(modelPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Vosk] Failed to load model: {e.Message}");
                return false;
            }
        }

        private void CaptureLoop()
        {
            var floatChunk = new float[ChunkSize];
            var shortChunk = new short[ChunkSize];
            int lastPos = 0;
            int clipSamples = _micClip.samples;

            while (_running)
            {
                int micPos;
                try { micPos = Microphone.GetPosition(_micDevice); }
                catch { break; }

                int available = micPos - lastPos;
                if (available < 0) available += clipSamples; // wrapped around the loop clip

                while (available >= ChunkSize && _running)
                {
                    try
                    {
                        _micClip.GetData(floatChunk, lastPos);
                    }
                    catch
                    {
                        // GetData can throw if the clip was torn down mid-read.
                        break;
                    }

                    float rms = VoiceUtility.ComputeRms(floatChunk, ChunkSize);
                    for (int i = 0; i < ChunkSize; i++)
                        shortChunk[i] = (short)Mathf.Clamp(floatChunk[i] * short.MaxValue, short.MinValue, short.MaxValue);

                    try
                    {
                        if (_recognizer.AcceptWaveform(shortChunk, ChunkSize))
                        {
                            string json = _recognizer.Result();
                            EmitFromJson(json, rms);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[Vosk] AcceptWaveform failed: {e.Message}");
                        _running = false;
                        break;
                    }

                    lastPos = (lastPos + ChunkSize) % clipSamples;
                    available -= ChunkSize;
                }

                Thread.Sleep(10); // ~100Hz poll; keeps latency well under the 150ms target
            }
        }

        /// <summary>Parse a Vosk JSON payload and, if it holds non-empty text, queue a result.</summary>
        private void EmitFromJson(string json, float lastRms)
        {
            if (string.IsNullOrEmpty(json))
                return;

            string text;
            try
            {
                var parsed = JsonUtility.FromJson<VoiceRecognizerJson>(json);
                text = parsed?.text;
            }
            catch
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
                return;

            var result = new VoiceRecognitionResult(
                rawText: text,
                normalizedText: VoiceUtility.Normalize(text),
                confidence: 1f,
                rmsAmplitude: lastRms,
                volume: VoiceUtility.ClassifyVolume(lastRms));

            _pending.Enqueue(result);
        }

        private void EnsurePump()
        {
            if (_pump != null)
                return;
            var go = new GameObject("~VoskVoicePump");
            go.hideFlags = HideFlags.HideAndDontSave;
            if (Application.isPlaying)
                UnityEngine.Object.DontDestroyOnLoad(go);
            _pump = go.AddComponent<MainThreadPump>();
            _pump.Init(this);
        }

        /// <summary>Called by the pump each frame on the main thread.</summary>
        internal void DrainPending()
        {
            while (_pending.TryDequeue(out var result))
            {
                Debug.Log($"[Vosk] Recognized {result}");
                OnPhraseRecognized?.Invoke(result);
            }
        }

        private void DisposeVosk()
        {
            try { _recognizer?.Dispose(); } catch { /* ignore */ }
            try { _model?.Dispose(); } catch { /* ignore */ }
            _recognizer = null;
            _model = null;
        }

        /// <summary>Hidden helper that pumps queued results onto the Unity main thread.</summary>
        private class MainThreadPump : MonoBehaviour
        {
            private VoskVoiceInputService _owner;
            public void Init(VoskVoiceInputService owner) => _owner = owner;
            private void Update() => _owner?.DrainPending();
        }
#endif
    }
}
