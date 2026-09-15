using UnityEngine;
using UnityEngine.Events;

namespace RogueAi.Voice
{
    /// <summary>
    /// Hold-to-talk driver. Holding the push-to-cast key opens the microphone
    /// (StartListening); releasing it closes the mic (StopListening), which is what
    /// triggers phrase recognition. The actual spell resolution happens in
    /// SpellCastingSystem, which subscribes to
    /// <see cref="IVoiceInputService.OnPhraseRecognized"/> — this controller is
    /// intentionally decoupled from the Spells assembly (Spells depends on Voice,
    /// not the other way around) so it only drives listening + visual feedback.
    /// </summary>
    public class PushToCastController : MonoBehaviour
    {
        [Header("Input")]
        [Tooltip("Hold to capture voice, release to cast.")]
        [SerializeField] private KeyCode _pushToCastKey = KeyCode.V;

        [Header("Visual feedback (optional)")]
        [Tooltip("Animator whose bool parameter is toggled while casting (e.g. hand raise).")]
        [SerializeField] private Animator _handAnimator;
        [SerializeField] private string _isCastingBool = "IsCasting";

        [Tooltip("UI GameObject (e.g. a 'listening' indicator) shown only while casting.")]
        [SerializeField] private GameObject _castingIndicator;

        [Tooltip("Raised whenever the casting/listening state changes. Wire UI or SFX here.")]
        public UnityEvent<bool> OnCastingStateChanged = new UnityEvent<bool>();

        /// <summary>True while the push-to-cast key is held and the mic is open.</summary>
        public bool IsCasting { get; private set; }

        private int _isCastingHash;

        private void Awake()
        {
            _isCastingHash = Animator.StringToHash(_isCastingBool);
            SetCastingVisual(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_pushToCastKey))
                BeginCasting();
            else if (Input.GetKeyUp(_pushToCastKey))
                EndCasting();
        }

        private void BeginCasting()
        {
            if (IsCasting)
                return;
            IsCasting = true;

            var service = VoiceServiceLocator.Current;
            service?.StartListening();

            SetCastingVisual(true);
            OnCastingStateChanged?.Invoke(true);
        }

        private void EndCasting()
        {
            if (!IsCasting)
                return;
            IsCasting = false;

            var service = VoiceServiceLocator.Current;
            service?.StopListening(); // <- this is what fires OnPhraseRecognized

            SetCastingVisual(false);
            OnCastingStateChanged?.Invoke(false);
        }

        private void SetCastingVisual(bool casting)
        {
            if (_handAnimator != null && !string.IsNullOrEmpty(_isCastingBool))
                _handAnimator.SetBool(_isCastingHash, casting);
            if (_castingIndicator != null)
                _castingIndicator.SetActive(casting);
        }

        private void OnDisable()
        {
            // Safety: never leave the mic open if the controller is torn down mid-cast.
            if (IsCasting)
                EndCasting();
        }
    }
}
