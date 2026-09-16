# Plunderspell — docs

Plunderspell is a four-player co-op heist: raid a procedurally generated castle, cast spells by
speaking Latin-ish words into a microphone, carry out what you can (it has real weight), and get
out before the alarm you triggered catches up with you. Built in Unity 6000.3.15f1 on PurrNet
(networking) and Vosk (offline speech), forked from an earlier third-person extraction-game
prototype (`RogueLikeSlop@ThirdPerson`) that this repo's own name and initial commits still
reflect.

Nobody should have to search this folder — if something isn't linked from here, that's a gap in
this document, not a reason to go grep the repo.

## The moving parts

| Path | |
|---|---|
| `Assets/_Project/Scripts/Runtime/` | all gameplay, one assembly per system |
| `Assets/_Project/Scripts/Tests/` | the test suite (runs in Unity and headlessly) |
| `Assets/_Project/Scripts/Editor/` | scene builders and dev tooling |
| `Tools/Headless/` | headless build + test harness — compiles/tests without a Unity install |
| `Tools/AssetPipeline/`, `Tools/EnemyForge/` | Blender-driven generation of props and enemy models |
| `docs/` | this tree |

## The tiers

| Tier | File | Answers |
|---|---|---|
| 1 | `docs/README.md` | What is this, where is everything (this document) |
| 2 | [`docs/Roadmap.md`](Roadmap.md) | What 0–100% means, per milestone |
| 3 | [`docs/ProjectState.md`](ProjectState.md) | Where it actually stands right now |
| 4 | [`docs/systems/`](systems/README.md) | How each runtime-critical system works |
| 5 | [`docs/Today.md`](Today.md) | What's being worked on today, and why |

Plus `docs/plans/` (a plan for a specific piece of work, live until executed) and
`docs/archive/` (documents that were correct once and are now inert — moved, never deleted).

## Systems (tier 4)

See [`docs/systems/README.md`](systems/README.md) for the full index, including what was folded
together and what was deliberately left undocumented, and why.

| System | Owns |
|---|---|
| [`core`](systems/core.md) | Singleton base, event bus, state machine contract |
| [`net`](systems/net.md) | Steam invite plumbing (cold launch, rich presence) |
| [`voice`](systems/voice.md) | Held-key + microphone audio → a recognised phrase |
| [`spells`](systems/spells.md) | Resolving a phrase into a spell, a misfire, or a fizzle |
| [`castle`](systems/castle.md) | Deterministic seed-driven castle layout and path validation |
| [`alarm`](systems/alarm.md) | Noise propagation and the castle-wide alarm state machine |
| [`raid`](systems/raid.md) | The loop: Lair → castle → haul → extraction → Lair |
| [`enemy-asset-pipeline`](systems/enemy-asset-pipeline.md) | Generating the enemy roster from Python/Blender |

## Everything else worth reaching

- [`docs/plunderspell.md`](plunderspell.md) — the pitch/design bible (v0.2). Written to be read
  aloud; §9 onward is plain fact. Illustrated mood board: `docs/plunderspell-moodboard.html`.
- [`docs/plans/plunderspell.md`](plans/plunderspell.md) — the engineering plan the roadmap was
  built from, including the provenance constraint around the excluded `feature/Owen/PCG` branch.
- [`docs/plans/steam-coop-framework.md`](plans/steam-coop-framework.md) — the plan for porting the
  predecessor project's Steam co-op framework, which Plunderspell was forked from.
- [`docs/prompts/plunderspell-fable.md`](prompts/plunderspell-fable.md) — the build-and-test
  prompt used to drive an agent session on this project.
- `docs/castle-generator-visualization.html`, `docs/ui-preview.html` — standalone interactive/HTML
  previews; open directly in a browser.
- `Tools/Headless/README.md` — what the headless verification harness does and does not prove.
- `Tools/AssetPipeline/README.md` — the reproducible Blender prop pipeline.
- [`docs/archive/`](archive/README.md) — inert documents kept for the record, starting with the
  full paper trail from the 2026-09-15 branch integration.

## Conventions

- **Cite claims to `file:line`** where practical — it's what makes a doc's claims checkable
  instead of a matter of opinion.
- **A milestone is done when its acceptance criterion has been checked, not when the code
  exists.** See `docs/ProjectState.md` for the current gap between the two.
- **A doc that's gone inert moves to `docs/archive/`. It does not get deleted.** Fix the pointers
  into it rather than leaving them broken.
- **Update the tier that changed.** Tier 3 (`ProjectState.md`) moves often; tier 2
  (`Roadmap.md`) only when the definition of "done" itself changes.
