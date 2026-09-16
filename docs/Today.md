# Today

**2026-09-16 — documentation day.** No code changed. `main` had just received the full
Plunderspell build (95 commits, PR #3, `integration/staging-2026-09-15` → `main`), and `docs/`
had tier 4 (`docs/systems/`) and plans already but no tier 1, 2, 3 or 5, and no `archive/`.

## Done

- Wrote tier 1 (`docs/README.md`), tier 2 (`docs/Roadmap.md`), tier 3 (`docs/ProjectState.md`).
- Added three tier-4 system docs that were missing for genuinely load-bearing, previously
  undocumented systems: `voice.md`, `castle.md`, `alarm.md` (bundling Acoustics + Alarm — the
  third named pillar of the pitch had no doc at all before today).
- Wrote `docs/systems/README.md` indexing all eight tier-4 docs, with an explicit list of what
  was folded together (Guards/UI/Status/Playtest/Loot/Extraction/Lair/Inventory into `raid.md`)
  and what was left out entirely (Player/Enemies — generic ported FSM infra, no
  Plunderspell-specific decision to record).
- Moved `.agent_reports/` (the 2026-09-15 integration's paper trail — merge narrative, known
  issues, raw test XML) into `docs/archive/2026-09-15-integration/` with a `README.md` explaining
  why it's there, and fixed the one stale self-reference inside it.
- Found and fixed one real drift: `docs/plans/plunderspell.md`'s own "Status" section still said
  "Planning complete; no implementation yet" after M0–M3 had already merged. Rewrote it to point
  at `docs/ProjectState.md` instead of leaving the old claim standing.

## Deliberately not done

- Did not write per-file doc comments or touch any `.cs` file — this was a documentation-structure
  pass, not a code-quality pass.
- Did not attempt to verify M1/M2's acceptance criteria myself (real-microphone accent testing,
  a real four-player session) — those need a human with a microphone and three friends, not a
  docs pass. Flagged as unchecked in `docs/ProjectState.md` instead of quietly marking them done.

## Surfaced, not today's job

- `docs/ProjectState.md` → "Choosing an era does nothing to the raid you get" — `CastleRoomRegistry`
  has no era field, so M3 needs a data-model change before it can produce era-specific content.
  This is a real finding, not a doc nit; whoever picks up M3 should read that section first.
- No real Unity Editor player build has ever been produced for this project (confirmed in the
  archived `FINAL_MERGE_SUMMARY.md` — there is no `BuildPipeline.BuildPlayer()` entry point
  anywhere in `Assets/`). Whether it actually runs as a standalone build is unknown.

## Next, in order

1. Whoever works on M1 next should decide whether to measure the real acceptance criterion
   (multi-accent recognition accuracy, latency) before calling M1 done, or to explicitly revise
   the criterion — see `docs/Roadmap.md`'s note on that.
2. Same for M2: get an actual four-player raid played start to finish and record what happened,
   rather than relying on the automated PlayMode suite as a stand-in for "is this fun."
3. Only after 1–2: decide the `CastleRoomRegistry`/loot/guard schema change M3 actually needs.
