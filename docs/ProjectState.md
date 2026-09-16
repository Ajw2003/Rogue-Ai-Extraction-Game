# Project State

**Headline: ~60% against the roadmap in `docs/Roadmap.md`.** Every milestone's code has been
written, merged, and passes an automated test suite. Two of the four milestones' acceptance
criteria have never actually been checked the way they're defined — see below.

| Milestone | Status | Code | Acceptance checked? |
|---|---|---|---|
| M0 — Fork clean, cut gravity | Done | ✅ merged (`feature/m0-gravity-removal`) | ✅ — compiles, gravity restored, verified in `.agent_reports`-era logs, now `docs/archive/2026-09-15-integration/` |
| M1 — Prove the voice | Code complete, acceptance unchecked | ✅ merged (`feature/m1-voice-casting`) | ❌ — no real-microphone, multi-accent, latency measurement exists anywhere in the repo |
| M2 — The vertical slice | Code complete, acceptance unchecked | ✅ merged (`feature/m2-castle-gen`, `feature/m2-loot-acoustics`) | ❌ — 105/105 automated PlayMode tests pass; no record of four real people playing a raid together |
| M3 — Open the other Ages | Scaffold only | 🟡 `HistoricalEra` enum + plumbing only | ❌ — see below, the data model can't produce era-specific content yet |

## The one thing that is not what it looks like

**Choosing an era in the Lair does nothing to the raid you get.** `RaidDirector.StartRaid(era)`
takes a `HistoricalEra`, stores it, and forwards it to `LairHubManager.SelectEra`. It reads as a
finished feature — the Lair has era selection UI-adjacent state, `RaidDirector` has an `Era`
property, everything compiles and the tests pass. But `CastleRoomRegistry` (see
`docs/systems/castle.md`) tags every room module only by `CastleZone`, with no era field at all,
and neither the loot planner nor the guard planner branch on era anywhere (`grep -rn
"HistoricalEra" Assets/_Project/Scripts/Runtime/Castle Assets/_Project/Scripts/Runtime/Loot
Assets/_Project/Scripts/Runtime/Guards` returns nothing). Every raid, in every era, currently
builds from the same single room set, loot table and guard roster. M3's acceptance criterion —
"a different era produces a measurably different raid" — is not close to met; it needs a schema
change (`CastleRoomModuleData.Era`, era-keyed loot/guard tables) before it's even possible, not
just more content.

## Cross-cutting issues that belong to no milestone

- **The `isSpawned`/`isServer` trap has already caused three separate silent failures** (voice
  casting, the extraction clock, trigger tracking — see `docs/systems/raid.md` and
  `docs/systems/voice.md`) because `if (!isServer) return;` is true on an unspawned object as well
  as a real client. All three known instances are fixed (`if (isSpawned && !isServer) return;`),
  but the underlying trap is a property of PurrNet's authority model, not something the codebase
  can permanently rule out — any new `NetworkBehaviour` is at risk of the same bug on its first
  offline/single-player run.
- **No real Unity Editor player build has ever been produced or checked.** `FINAL_MERGE_SUMMARY`
  (now `docs/archive/2026-09-15-integration/`) confirms this project has no
  `BuildPipeline.BuildPlayer()` entry point anywhere — only EditMode/PlayMode test runs are
  verified. Whether the project actually builds and runs as a standalone player is unknown.
- **`docs/plans/plunderspell.md`'s own "Status" section says "Planning complete; no implementation
  yet."** That was true when written and is no longer true — M0 through M3 are all merged. It has
  not been updated to point here.
