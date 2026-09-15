using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueAi.Voice
{
    /// <summary>
    /// Keyboard-driven stand-in for the real speech recogniser. Used in the editor and
    /// headless CI so the whole casting pipeline can be exercised without a microphone
    /// or the native Vosk binary.
    ///
    /// While listening:
    ///   Alpha1..Alpha8            -> correct spell words
    ///   Shift + Alpha1..Alpha8    -> deliberate misfire near-matches
    ///   Ctrl  held  -> whisper (rms 0.05)
    ///   Shift held  -> shout   (rms 0.6)   [Shift also selects the misfire word]
    ///   otherwise   -> normal  (rms 0.15..0.35)
    /// </summary>
    public class MockVoiceInputService : MonoBehaviour, IVoiceInputService
    {
        public bool IsListening { get; private set; }
        public event Action<VoiceRecognitionResult> OnPhraseRecognized;

        /// <summary>Correct spell word per key.</summary>
        public readonly Dictionary<KeyCode, string> keybindMap = new Dictionary<KeyCode, string>
        {
            { KeyCode.Alpha1, "IGNIS" },
            { KeyCode.Alpha2, "FRANGO" },
            { KeyCode.Alpha3, "LEVO" },
            { KeyCode.Alpha4, "AURUM VOCO" },
            { KeyCode.Alpha5, "TONITRUS" },
            { KeyCode.Alpha6, "SOMNUS" },
            { KeyCode.Alpha7, "CADAVER SURGE" },
            { KeyCode.Alpha8, "PORTA" },
        };

        /// <summary>Near-match / misfire word per key (fired when Shift is held).</summary>
        public readonly Dictionary<KeyCode, string> misfireMap = new Dictionary<KeyCode, string>
        {
            { KeyCode.Alpha1, "AGNIS" },
            { KeyCode.Alpha2, "FRANCO" },
            { KeyCode.Alpha3, "LAVO" },
            { KeyCode.Alpha4, "AURUM BOCO" },
            { KeyCode.Alpha5, "TONITUS" },
            { KeyCode.Alpha6, "SONUS" },
            { KeyCode.Alpha7, "CADAVER SURJ" },
            { KeyCode.Alpha8, "PROTA" },
        };

        private static MockVoiceInputService _instance;

        /// <summary>Get (or lazily create a hidden driver object for) the mock service.</summary>
        public static MockVoiceInputService GetOrCreate()
        {
            if (_instance != null)
                return _instance;

            var go = new GameObject("~MockVoiceInputService");
            go.hideFlags = HideFlags.HideAndDontSave;
            if (Application.isPlaying)
                DontDestroyOnLoad(go);
            _instance = go.AddComponent<MockVoiceInputService>();
            return _instance;
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public void StartListening()
        {
            IsListening = true;
            Debug.Log("[MockVoice] StartListening");
        }

        public void StopListening()
        {
            IsListening = false;
            Debug.Log("[MockVoice] StopListening");
        }

        private void Update()
        {
            if (!IsListening)
                return;

            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            foreach (var kv in keybindMap)
            {
                if (Input.GetKeyDown(kv.Key))
                {
                    string word = shift && misfireMap.TryGetValue(kv.Key, out var mis) ? mis : kv.Value;
                    EmitPhrase(word, ctrl, shift);
                }
            }
        }

        /// <summary>
        /// Public test seam: synthesise a phrase for a key press exactly as Update() would,
        /// so unit tests never depend on the Input system.
        /// </summary>
        public void SimulateKeyPress(KeyCode key, bool shift = false, bool ctrl = false)
        {
            if (!IsListening)
                return;
            string word = shift && misfireMap.TryGetValue(key, out var mis)
                ? mis
                : (keybindMap.TryGetValue(key, out var w) ? w : null);
            if (word == null)
                return;
            EmitPhrase(word, ctrl, shift);
        }

        private void EmitPhrase(string rawWord, bool ctrl, bool shift)
        {
            float rms;
            if (ctrl) rms = 0.05f;         // whisper
            else if (shift) rms = 0.6f;    // shout
            else rms = UnityEngine.Random.Range(0.15f, 0.35f); // normal

            var result = new VoiceRecognitionResult(
                rawText: rawWord,
                normalizedText: VoiceUtility.Normalize(rawWord),
                confidence: 1f,
                rmsAmplitude: rms,
                volume: VoiceUtility.ClassifyVolume(rms));

            Debug.Log($"[MockVoice] Emit {result}");
            OnPhraseRecognized?.Invoke(result);
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
