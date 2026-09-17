# Project State

**Headline: ~65% against the roadmap in `docs/Roadmap.md`.** Every milestone's code has been
written, merged, and passes an automated test suite; the raid now runs on real authored art
instead of primitives. Two of the four milestones' acceptance criteria have never actually been
checked the way they're defined, and a 21-item playtesting backlog (filed 2026-09-16, all still
open — see below) is the clearest evidence of the gap between "compiles and passes tests" and
"plays like the pitch."

| Milestone | Status | Code | Acceptance checked? |
|---|---|---|---|
| M0 — Fork clean, cut gravity | Done | ✅ merged (`feature/m0-gravity-removal`) | ✅ — compiles, gravity restored, verified in the `.agent_reports`-era logs, now `docs/archive/2026-09-15-integration/` |
| M1 — Prove the voice | Code complete, acceptance unchecked | ✅ merged (`feature/m1-voice-casting`) | ❌ — no real-microphone, multi-accent, latency measurement exists anywhere in the repo |
| M2 — The vertical slice | Code complete, real art wired in, acceptance unchecked | ✅ merged; the raid scene now assembles from 25 castle rooms, 5 loot prefabs and 10 enemy prefabs instead of primitives (`docs/systems/raid-scene-assembly.md`), and the menu → lair → raid → lair flow is live (`fc22668`) | ❌ — 116/116 automated tests pass; no record of four real people playing a raid together, and the 2026-09-16 playtesting backlog (below) found 21 rough edges standing between the built loop and something you'd hand a friend |
| M3 — Open the other Ages | Scaffold only | 🟡 `HistoricalEra` enum + plumbing only | ❌ — see below, the data model can't produce era-specific content yet |

## The one thing that is not what it looks like

**Choosing an era in the Lair does nothing to the raid you get.** `RaidDirector.StartRaid(era)`
takes a `HistoricalEra`, stores it, and forwards it to `LairHubManager.SelectEra`. It reads as a
finished feature — the Lair has era selection UI-adjacent state, `RaidDirector` has an `Era`
property, everything compiles and the tests pass. But `CastleRoomRegistry` (see
`docs/systems/castle.md`) tags every room module only by `CastleZone`, with no era field at all,
and neither the loot planner nor the guard planner branch on era anywhere (`grep -rn
"HistoricalEra" Assets/_Project/Scripts/Runtime/Castle Assets/_Project/Scripts/Runtime/Loot
Assets/_Project/Scripts/Runtime/Guards` returns nothing — still true as of this pass). Every raid,
in every era, currently builds from the same single room set, loot table and guard roster. M3's
acceptance criterion — "a different era produces a measurably different raid" — is not close to
met; it needs a schema change (`CastleRoomModuleData.Era`, era-keyed loot/guard tables) before
it's even possible, not just more content.

## The 2026-09-16 playtesting backlog

Filed as GitHub issues #5–#25 (`Tools/mkissues.py`, manifest in
`docs/generated/github-issues.json`) immediately after the raid scene started assembling from real
art — so these are gaps the art exposed, not pre-art complaints. All 21 are open as of this pass
(`gh issue list --state all`); #24 is the epic tying the rest together. The ones most worth reading
before touching the raid loop:

- **#20 — loot is flung across the map by physics at spawn.** Already documented as an open,
  unfixed defect in `docs/systems/raid-scene-assembly.md` ("Traps") — roughly 2–7 of ~19 pieces
  per raid. An attempted floor-raycast fix made it worse (15/22) and was reverted; a real fix needs
  the spawner to find a clear resting spot while keeping the placement planner pure.
- **#5 / #19 — rooms don't connect / modules float and snap inconsistently.** Filed against the
  same generator `docs/systems/castle.md` describes; `CastlePathValidator` guarantees the *layout*
  is reachable, not that the *meshes* read as continuous interior space.
- **#21 — no portal asset**, **#16 — no main menu art**, **#23 — castles look bland**, **#22 — no
  VFX/SFX anywhere** — the game loop runs end to end but almost nothing in it has a finished visual
  or audio pass yet.
- **#6 / #25 — player scale and spawn placement** — the player can be too tall for the rooms, and
  can spawn inside or flush against castle geometry.

None of these are tracked against a specific milestone above; they're cross-cutting polish and
correctness gaps surfaced by actually looking at the built scene; see "Cross-cutting issues" below
for one further failure mode of the same kind.

## Cross-cutting issues that belong to no milestone

- **The `isSpawned`/`isServer` trap has already caused three separate silent failures** (voice
  casting, the extraction clock, trigger tracking — see `docs/systems/raid.md`,
  `docs/systems/voice.md` and `docs/systems/alarm.md`) because `if (!isServer) return;` is true on
  an unspawned object as well as a real client. All three known instances are fixed
  (`if (isSpawned && !isServer) return;`), but the underlying trap is a property of PurrNet's
  authority model, not something the codebase can permanently rule out — any new
  `NetworkBehaviour` is at risk of the same bug on its first offline/single-player run.
- **No real Unity Editor player build has ever been produced or checked.** There is still no
  `BuildPipeline.BuildPlayer()` entry point anywhere in `Assets/` (confirmed by grep as of this
  pass). Whether the project actually builds and runs as a standalone player is unknown.
