# Today

**2026-09-16 — documentation audit day.** No gameplay code changed. `main` had gained the
raid-scene-assembly work, the menu → lair → raid flow, the castle orientation fix and a 21-issue
playtesting backlog since anyone last touched `docs/`, and `docs/` itself had tier 4
(`docs/systems/`) and `docs/plans/` but no tier 1, 2, 3, 5 or 6, and no `archive/` or `generated/`.

## Done

- Found `origin/claude/repo-status-check-hjp6z7`: an unmerged branch that had already written
  tiers 1, 2, 3, 5 and three missing tier-4 docs (`voice.md`, `castle.md`, `alarm.md`), cut from a
  point 11 commits behind current `main`. Adopted its tier-4 work as-is (the systems it covers
  hadn't changed) and rewrote the rest for what's shipped since. See `docs/Decisions.md`'s first
  entry for the full reasoning.
- Wrote tier 6 (`docs/Decisions.md`) from scratch — 8 real, dated decisions mined from commit
  history and the existing plan/system docs, none invented.
- Added `docs/systems/raid-scene-assembly.md` to the tier-4 index (`docs/systems/README.md`) — it
  already existed on `main` but was never indexed.
- Created `docs/generated/` and moved the three standalone HTML previews
  (`castle-generator-visualization.html`, `plunderspell-moodboard.html`, `ui-preview.html`) into
  it from the `docs/` root, plus the live GitHub-issues manifest
  (`.agent_reports/github-issues.json` → `docs/generated/github-issues.json`), updating
  `Tools/mkissues.py`'s output path and every cross-reference into the moved files.
- Moved the remaining `.agent_reports/` paper trail (the 2026-09-15 branch-integration narrative
  and test results — the docs-scaffold branch had already moved these on its own copy, but that
  branch was never merged so `main` still had the original `.agent_reports/` at the repo root)
  into `docs/archive/2026-09-15-integration/`, with the same "not actually committed" correction
  to `FINAL_MERGE_SUMMARY.md`'s self-reference that the abandoned branch had already worked out.
- Updated `docs/ProjectState.md`: the raid now runs on real authored art rather than primitives,
  the menu/lair/raid flow is live, and the 2026-09-16 issue backlog (21 open items, verified live
  via `gh issue list`) is now the concrete evidence behind M2's "acceptance unchecked" status.
- Fixed the one real drift already known from the abandoned branch: `docs/plans/plunderspell.md`'s
  own "Status" section still said "Planning complete; no implementation yet" despite M0–M3 having
  merged weeks ago. Pointed it at `docs/ProjectState.md` instead.
- Added a pointer from the project's `CLAUDE.md` to `docs/README.md`.

## Deliberately not done

- Did not write per-file doc comments or touch any `.cs` file beyond the two path corrections
  above (`Tools/mkissues.py`'s output directory) — this was a documentation-structure pass, not a
  code-quality pass.
- Did not attempt to verify M1/M2's acceptance criteria myself (real-microphone accent testing, a
  real four-player session), and did not triage the 21 open GitHub issues individually — both need
  a human (and, for the issues, actual engineering time), not a docs pass. Flagged as unchecked /
  open in `docs/ProjectState.md` instead of guessing at status.

## Surfaced, not today's job

- `docs/ProjectState.md` → "Choosing an era does nothing to the raid you get" — still true,
  re-verified by grep this pass. `CastleRoomRegistry` has no era field, so M3 needs a data-model
  change before it can produce era-specific content.
- The 21-issue playtesting backlog (`docs/generated/github-issues.json`, GitHub issues #5–#25) is
  a real, current punch list — #20 (loot physics fling) and #5/#19 (room connectivity/floating
  modules) are the ones most likely to block a real four-player playtest of M2's acceptance
  criterion.
- No real Unity Editor player build has ever been produced for this project — reconfirmed by grep
  (`BuildPipeline.BuildPlayer` appears nowhere in `Assets/`). Whether it actually runs as a
  standalone build is still unknown.

## Next, in order

1. Triage the 21 open GitHub issues against the milestones in `docs/Roadmap.md` — several (loot
   fling, room connectivity, player spawn placement) block a credible M2 playtest; others (VFX/SFX,
   menu art) don't block the loop but do block "something you'd hand a friend."
2. Get an actual four-player raid played start to finish and record what happened, rather than
   relying on the automated PlayMode suite as a stand-in for "is this fun" — same for M1's
   real-microphone, multi-accent recognition measurement.
3. Only after 1–2: decide the `CastleRoomRegistry`/loot/guard schema change M3 actually needs.
