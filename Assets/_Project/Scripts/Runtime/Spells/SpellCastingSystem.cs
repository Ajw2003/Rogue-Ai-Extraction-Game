using PurrNet;
using RogueAi.Voice;
using UnityEngine;

namespace RogueAi.Spells
{
    /// <summary>
    /// Bridges the voice pipeline to networked spell casting.
    ///
    /// On the owning client it listens for recognised phrases, resolves them through the
    /// <see cref="MisfireEngine"/> against the <see cref="SpellLexicon"/>, then asks the
    /// server to broadcast the cast so every client logs the same outcome. Actual spell
    /// effect execution is a future milestone — for now this is a networked logging stub.
    /// </summary>
    public class SpellCastingSystem : NetworkBehaviour
    {
        [Tooltip("The spellbook used to resolve spoken phrases into spells/misfires.")]
        [SerializeField] private SpellLexicon _lexicon;

        private IVoiceInputService _voice;
        private bool _subscribed;

        protected override void OnSpawned()
        {
            base.OnSpawned();

            // Only the local owner captures voice; remote copies just receive broadcasts.
            if (!isOwner)
                return;

            if (_lexicon == null)
                Debug.LogWarning("[SpellCast] No SpellLexicon assigned — every phrase will fizzle (None).");

            _voice = VoiceServiceLocator.Current;
            if (_voice != null)
            {
                _voice.OnPhraseRecognized += HandlePhrase;
                _subscribed = true;
            }
            else
            {
                Debug.LogWarning("[SpellCast] No voice service available.");
            }
        }

        protected override void OnDespawned()
        {
            base.OnDespawned();
            Unsubscribe();
        }

        private void OnDisable() => Unsubscribe();

        private void Unsubscribe()
        {
            if (_subscribed && _voice != null)
                _voice.OnPhraseRecognized -= HandlePhrase;
            _subscribed = false;
        }

        /// <summary>Owner-side handler: resolve the phrase and request a networked cast.</summary>
        private void HandlePhrase(VoiceRecognitionResult result)
        {
            SpellId resolved = MisfireEngine.Resolve(result, _lexicon);
            if (resolved == SpellId.None)
            {
                Debug.Log($"[SpellCast] Phrase \"{result.NormalizedText}\" fizzled (no match).");
                return;
            }

            bool isMisfire = IsMisfire(resolved);
            if (isMisfire)
                Debug.Log($"[Misfire] Local cast misfired \"{result.NormalizedText}\" \u2192 {resolved} (Volume: {result.Volume})");
            else
                Debug.Log($"[SpellCast] Local cast {resolved} (Volume: {result.Volume})");

            // Ask the server to broadcast this cast to everyone (incl. us) for a shared log.
            ServerCast(resolved, result.Volume, this);
        }

        /// <summary>
        /// Server entry point. Validates trivially then fans the cast out to all observers.
        /// Effect execution will be added in a later milestone; today it just broadcasts a log.
        /// </summary>
        [ServerRpc(requireOwnership: true)]
        private void ServerCast(SpellId spellId, CastVolume volume, NetworkIdentity caster, RPCInfo info = default)
        {
            // info.sender is the player that requested the cast.
            BroadcastCast(spellId, volume, caster, info.sender);
        }

        /// <summary>Runs on every client (and host). Logs the cast so all peers agree on the outcome.</summary>
        [ObserversRpc(bufferLast: false)]
        private void BroadcastCast(SpellId spellId, CastVolume volume, NetworkIdentity caster, PlayerID sender)
        {
            string who = caster != null ? caster.name : sender.ToString();
            if (IsMisfire(spellId))
                Debug.Log($"[Misfire] Player {who} misfired \u2192 {spellId} (Volume: {volume})");
            else
                Debug.Log($"[SpellCast] Player {who} cast {spellId} (Volume: {volume})");

            // TODO(M-later): dispatch to spell effect execution here.
        }

        /// <summary>True if the resolved id is one of the misfire outcomes.</summary>
        public static bool IsMisfire(SpellId id) => (int)id >= 100;
    }
}
