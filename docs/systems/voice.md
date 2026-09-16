# Voice

The first pillar of the pitch: casting is speaking, not pressing a button. `RogueAi.Voice`
provides that input and nothing downstream of it — spell resolution belongs to `Spells`.

## What it owns

Turning a held key + microphone audio into a `VoiceRecognitionResult` (normalised text,
confidence, RMS amplitude, loudness bucket) and handing it to whatever subscribes
(`SpellCastingSystem`, in `RogueAi.Spells`). Nothing else — Voice does not know what a spell word
means or what happens when one misfires.

## How it works

- **`IVoiceInputService`** (`IVoiceInputService.cs`) is the only surface consumers see:
  `StartListening()` / `StopListening()` / `OnPhraseRecognized`. Two implementations exist behind
  it, chosen automatically:
  - **`VoskVoiceInputService`** — the real path. `Microphone.Start()` captures 16 kHz mono audio;
    a background thread feeds 4096-sample chunks to an offline Vosk recognizer and parses its JSON
    result; the finished `VoiceRecognitionResult` is marshalled back to the main thread. All Vosk
    and `Microphone` calls are wrapped in `#if !HEADLESS` so dedicated-server/CI builds compile
    without the native binary and just stay silent.
  - **`MockVoiceInputService`** — a keyboard stand-in (`1`–`8` while holding the cast key) used in
    editor, headless builds, and anywhere no microphone is available.
- **`VoiceServiceLocator.Current`** auto-registers the right one on first access, checked in this
  order: editor → always mock; `HEADLESS` define → always mock; otherwise batch mode, a null
  graphics device, or zero microphone devices → mock; else the real Vosk service. Any caller
  (tests included) can override with `Register()` before first use.
- **`PushToCastController`** is the hold-to-talk driver: press opens the mic, release closes it —
  closing is what triggers recognition. It only drives listening and visual feedback
  (`OnCastingStateChanged`); it deliberately does not depend on `RogueAi.Spells` (dependency runs
  the other way), so a UI/animation change here can never touch spell logic.
- **`VoiceUtility`** is shared by both providers so they classify identically: `ClassifyVolume`
  buckets RMS into `Whisper` (< 0.1) / `Normal` / `Shout` (> 0.4); `Normalize` upper-cases, strips
  punctuation and collapses whitespace before the text ever reaches the lexicon in `Spells`.

## Invariants

- **Both providers must classify and normalise identically.** `VoiceUtility` is the single place
  either one is allowed to do it — a provider that rolls its own would make Whisper/Shout and
  misfire matching behave differently depending on which one is active, including between a dev's
  editor session and a shipped Windows build.
- **`OnPhraseRecognized` only fires on the main thread.** `VoskVoiceInputService`'s worker thread
  queues results and a `MainThreadPump` drains them; nothing downstream (`SpellCastingSystem`,
  UI) is written to expect a background-thread call.

## Traps

- **`VoiceServiceLocator` never auto-registers at editor load**, on purpose — doing so would spawn
  a hidden driver `GameObject` in every edit-mode session with no play running. It only
  auto-registers from `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` or lazily on first
  `Current` access. Code that expects a provider to exist before play mode starts will find none.
- **This is the system the `isServer`/unspawned-object trap (see `raid.md`) hit hardest.** Before
  it was fixed, `OnSpawned` never fired offline, so nothing ever subscribed to
  `OnPhraseRecognized` in single-player — the mic opened and closed and nothing happened at all,
  with no error anywhere in the chain to point at why.
- **A machine with no microphone silently downgrades to the keyboard mock**, even in a real
  Windows build — there is no user-facing warning that voice casting isn't actually listening.
