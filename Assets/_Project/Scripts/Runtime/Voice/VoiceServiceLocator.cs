using UnityEngine;

namespace RogueAi.Voice
{
    /// <summary>
    /// Static access point for the active <see cref="IVoiceInputService"/>.
    ///
    /// Auto-registration policy (runs before the first scene loads):
    ///   - In the editor, on a headless/batch build, or with no audio device  -> <see cref="MockVoiceInputService"/>
    ///   - Otherwise (a real Windows x64 player)                              -> <see cref="VoskVoiceInputService"/>
    ///
    /// Any code may override the choice by calling <see cref="Register"/> before use
    /// (tests do exactly this).
    /// </summary>
    public static class VoiceServiceLocator
    {
        private static IVoiceInputService _current;

        /// <summary>The active provider. Auto-initialises on first access if nothing registered.</summary>
        public static IVoiceInputService Current
        {
            get
            {
                if (_current == null)
                    AutoRegister();
                return _current;
            }
        }

        /// <summary>True once a provider has been chosen.</summary>
        public static bool HasService => _current != null;

        /// <summary>Explicitly install a provider (overrides auto-registration).</summary>
        public static void Register(IVoiceInputService service)
        {
            _current = service;
            Debug.Log($"[VoiceServiceLocator] Registered provider: {service?.GetType().Name ?? "null"}");
        }

        /// <summary>Drop the current provider (mainly for test teardown).</summary>
        public static void Clear()
        {
            if (_current != null && _current.IsListening)
                _current.StopListening();
            _current = null;
        }

        // NOTE: we deliberately do NOT auto-register at editor load — that would spawn a
        // hidden driver GameObject in every edit-mode session. Play-mode registration runs
        // below; edit-mode / test callers get a provider lazily via Current.

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInit() => AutoRegister();

        private static void AutoRegister()
        {
            if (_current != null)
                return;

            if (ShouldUseMock())
            {
                _current = MockVoiceInputService.GetOrCreate();
                Debug.Log("[VoiceServiceLocator] Auto-registered MockVoiceInputService (editor/headless/no-mic).");
            }
            else
            {
                _current = new VoskVoiceInputService();
                Debug.Log("[VoiceServiceLocator] Auto-registered VoskVoiceInputService (Windows x64).");
            }
        }

        private static bool ShouldUseMock()
        {
#if UNITY_EDITOR
            return true;
#elif HEADLESS
            return true;
#else
            // Batch-mode / dedicated-server style launch, or a machine with no microphone.
            if (Application.isBatchMode)
                return true;
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
                return true;
            if (Microphone.devices == null || Microphone.devices.Length == 0)
                return true;
            return false;
#endif
        }
    }
}
