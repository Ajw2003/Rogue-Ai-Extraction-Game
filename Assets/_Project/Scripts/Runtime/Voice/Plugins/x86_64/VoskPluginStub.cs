// -----------------------------------------------------------------------------
// Vosk managed-API compile stub.
//
// The real Vosk C# bindings ship as a managed assembly (Vosk.dll) that P/Invokes
// the native libvosk.dll (Windows x64). Neither binary is committed to the repo
// (the model + native libs are ~40MB+ and are fetched via the editor downloader).
//
// This stub mirrors the public surface of the `Vosk` namespace so that
// VoskVoiceInputService and the rest of the project compile cleanly even when the
// real binaries are absent. Every member throws NotSupportedException at runtime.
//
// When the real Vosk.dll is dropped into Assets/Plugins/, add the scripting define
// symbol  VOSK_PRESENT  (Project Settings > Player > Scripting Define Symbols) so
// this stub compiles out and the genuine types are used instead.
// -----------------------------------------------------------------------------
#if !VOSK_PRESENT
using System;

namespace Vosk
{
    /// <summary>Stub mirror of Vosk.Model. Throws if actually used.</summary>
    public class Model : IDisposable
    {
        private const string Msg =
            "Vosk native/managed binaries are not present. Add Vosk.dll + libvosk.dll to " +
            "Assets/Plugins/x86_64 and define VOSK_PRESENT to enable real speech recognition.";

        public Model(string modelPath) => throw new NotSupportedException(Msg);
        public void Dispose() { }
    }

    /// <summary>Stub mirror of Vosk.SpkModel (speaker model). Throws if actually used.</summary>
    public class SpkModel : IDisposable
    {
        public SpkModel(string modelPath) =>
            throw new NotSupportedException("Vosk SpkModel unavailable (stub).");
        public void Dispose() { }
    }

    /// <summary>
    /// Stub mirror of Vosk.VoskRecognizer (the managed wrapper around the Kaldi
    /// recognizer). Signatures match the real bindings so calling code compiles.
    /// </summary>
    public class VoskRecognizer : IDisposable
    {
        private const string Msg = "VoskRecognizer unavailable (compile stub, define VOSK_PRESENT).";

        public VoskRecognizer(Model model, float sampleRate) => throw new NotSupportedException(Msg);
        public VoskRecognizer(Model model, float sampleRate, SpkModel spkModel) => throw new NotSupportedException(Msg);
        public VoskRecognizer(Model model, float sampleRate, string grammar) => throw new NotSupportedException(Msg);

        public void SetMaxAlternatives(int maxAlternatives) => throw new NotSupportedException(Msg);
        public void SetWords(bool words) => throw new NotSupportedException(Msg);
        public void SetPartialWords(bool partialWords) => throw new NotSupportedException(Msg);

        public bool AcceptWaveform(byte[] data, int len) => throw new NotSupportedException(Msg);
        public bool AcceptWaveform(short[] sdata, int len) => throw new NotSupportedException(Msg);
        public bool AcceptWaveform(float[] fdata, int len) => throw new NotSupportedException(Msg);

        public string Result() => throw new NotSupportedException(Msg);
        public string PartialResult() => throw new NotSupportedException(Msg);
        public string FinalResult() => throw new NotSupportedException(Msg);

        public void Reset() => throw new NotSupportedException(Msg);
        public void Dispose() { }
    }

    /// <summary>Stub mirror of the static Vosk.Vosk helper class.</summary>
    public static class Vosk
    {
        public static void SetLogLevel(int logLevel) { /* no-op stub */ }
        public static void GpuInit() { /* no-op stub */ }
        public static void GpuThreadInit() { /* no-op stub */ }
    }
}
#endif
