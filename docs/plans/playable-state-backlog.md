# Playable-state backlog

The organized, deduplicated, phase-ordered version of the full GitHub issue backlog, built to
answer one question: **what does it take to get Plunderspell to a state where four people can sit
down and play a raid together and it feels like the pitch, in the shortest path that doesn't
paint the project into a corner?**

Live until the backlog below is closed out or re-triaged; supersedes ad hoc issue-by-issue work.
Companion to [`docs/plans/moodboard-gap-closure.md`](moodboard-gap-closure.md) (the audit that
produced 34 of these 55 issues) and [`docs/ProjectState.md`](../ProjectState.md) (the milestone
scorecard this backlog moves). Each issue below has its own exhaustive plan in
[`docs/plans/issues/`](issues/); this document is the map, not the territory.

## Dedup pass (2026-09-17)

GitHub had **89 open issues**, but the moodboard-gap-closure backlog (`Tools/mkissues_moodboard_gap.py`)
had been filed twice — #26–59 and an exact re-run as #60–93, same titles, same bodies, same
labels. All 34 duplicates (#60–93) are now closed with `state_reason: duplicate`, pointing at
their original (`duplicate_of`). **55 unique open issues remain: #5–25 (the 2026-09-16 playtesting
backlog) and #26–59 (the moodboard-gap-closure backlog).** Nothing else in the backlog needed
deduplication — the two source audits were explicitly designed not to re-file each other's ground
(`docs/plans/moodboard-gap-closure.md`, "Method").

## How phases were decided

Ordered by **what blocks what**, not by label or issue number:

1. A bug that makes the castle unwalkable blocks every other feature from being evaluated in
   context, so it outranks any feature.
2. A missing core mechanic (health, combat, a portal) blocks the loop from being *completable*,
   which outranks feedback/juice on a loop nobody can finish yet.
3. Feedback and readability (VFX, SFX, animation, captions) make a completable loop legible,
   which is a precondition for a meaningful playtest.
4. The two milestone acceptance criteria (M1 voice, M2 four-player raid) should be **measured**
   once the loop is legible, not before (measuring an illegible loop just re-discovers phase 1–3)
   and not after everything else (a failed measurement should redirect later phases before more
   effort goes in).
5. Art/juice polish and the two missing pillars (Market, Lair-as-a-place) make the game match the
   pitch, but neither blocks a raid from being playable — they're what makes it feel like
   *Plunderspell* rather than merely *a working extraction game*.
6. Content decisions (bestiary fork, weapon roster, eras) are cross-cutting and cheaper to resolve
   before more art is built on top of an unresolved premise — deliberately not last, but not
   blocking either.

```mermaid
flowchart TB
    subgraph P0["Phase 0 — Unblock the loop (bugs)"]
        I5[#5 rooms don't connect]
        I19[#19 modules float/snap]
        I6[#6 player too tall]
        I25[#25 spawn in geometry]
        I20[#20 loot flung by physics]
        I8[#8 mouse not locked]
        I9[#9 menu doesn't pause input]
        I7[#7 no crosshair]
        I15[#15 loot not discoverable]
    end

    subgraph P1["Phase 1 — Close the loop (win/lose)"]
        I14[#14 no health/damage model]
        I18[#18 no combat test scene]
        I37[#37 no melee combat]
        I39[#39 no ranged aim/fire]
        I21[#21 no portal asset]
        I24[#24 EPIC full loop]
    end

    subgraph P2["Phase 2 — Make it readable (feedback)"]
        I12[#12 no damage VFX]
        I13[#13 no spell VFX]
        I22[#22 no VFX/SFX anywhere]
        I10[#10 enemy animations]
        I11[#11 player/door animations]
        I47[#47 loudness meter]
        I49[#49 recognised-phrase caption]
        I48[#48 casting invisible to teammates]
        I46[#46 portal countdown]
        I42[#42 guards silent]
        I51[#51 camera shake/hit-stop]
        I52[#52 throw/swing weight]
    end

    subgraph P3["Phase 3 — Measure the milestones"]
        I50[#50 M1 acceptance]
        I55[#55 M2 acceptance]
        I53[#53 standalone build]
        I54[#54 perf budget]
        I56[#56 settings completeness]
        I57[#57 keyboard-casting a11y]
    end

    subgraph P4["Phase 4 — Look/sound like the pitch"]
        I16[#16 main menu art]
        I23[#23 castles bland]
        I58[#58 Steam lobby art]
        I59[#59 six lighting frames]
        I34[#34 EPIC icon set]
        I40[#40 weapon/loot icons]
    end

    subgraph P5["Phase 5 — Content decisions + missing pillars"]
        I41[#41 bestiary fork - DECIDE FIRST]
        I38[#38 weapon roster]
        I17[#17 one era has art]
        I35[#35 era hazards]
        I36[#36 era loot catalogue]
        I43[#43 GildedColossus boss]
        I44[#44 door/socket hazards]
        I45[#45 drawbridge]
        I26[#26 EPIC Market]
        I30[#30 EPIC Lair-as-place]
    end

    P0 --> P1 --> P2 --> P3
    P2 --> P4
    P3 -.re-triages.-> P4
    P4 --> P5
    I41 -.blocks art on 5 enemies.-> P4
```

## Phase 0 — Unblock the loop

**Why first:** every one of these makes the castle physically or perceptually broken in a way
that invalidates testing anything downstream. #20 and #15 are linked (loot you can't reach because
it flew across the map). #6/#19/#5 are the same generator; #25/#8/#9/#7 are basic playable-state
hygiene that costs nothing to fix and blocks nothing else.

| # | Title | Labels | Plan |
|---|---|---|---|
| [5](https://github.com/Ajw2003/PlunderSpell/issues/5) | PCG: rooms don't connect | `pcg` `bug` | [issues/005-pcg-rooms-dont-connect.md](issues/005-pcg-rooms-dont-connect.md) |
| [19](https://github.com/Ajw2003/PlunderSpell/issues/19) | Modules float and snap inconsistently | `pcg` `bug` | [issues/019-modules-float-and-snap.md](issues/019-modules-float-and-snap.md) |
| [6](https://github.com/Ajw2003/PlunderSpell/issues/6) | Player too tall for the rooms | `gameplay` `bug` | [issues/006-player-too-tall.md](issues/006-player-too-tall.md) |
| [25](https://github.com/Ajw2003/PlunderSpell/issues/25) | Player spawn inside castle geometry | `gameplay` `bug` | [issues/025-player-spawn-in-geometry.md](issues/025-player-spawn-in-geometry.md) |
| [20](https://github.com/Ajw2003/PlunderSpell/issues/20) | Loot flung across the map by physics | `physics` `bug` | [issues/020-loot-flung-by-physics.md](issues/020-loot-flung-by-physics.md) |
| [15](https://github.com/Ajw2003/PlunderSpell/issues/15) | No carryable objects discoverable | `gameplay` `bug` | [issues/015-loot-not-discoverable.md](issues/015-loot-not-discoverable.md) |
| [8](https://github.com/Ajw2003/PlunderSpell/issues/8) | Mouse not locked or hidden | `ui` `bug` | [issues/008-mouse-not-locked.md](issues/008-mouse-not-locked.md) |
| [9](https://github.com/Ajw2003/PlunderSpell/issues/9) | Player moves while main menu open | `ui` `bug` | [issues/009-input-not-gated-by-menu.md](issues/009-input-not-gated-by-menu.md) |
| [7](https://github.com/Ajw2003/PlunderSpell/issues/7) | No crosshair | `ui` `enhancement` | [issues/007-no-crosshair.md](issues/007-no-crosshair.md) |

## Phase 1 — Close the loop

**Why second:** a raid that can't be won or lost isn't a game yet. These are the systems that
turn "walk around a pretty castle" into "raid it."

| # | Title | Labels | Plan |
|---|---|---|---|
| [14](https://github.com/Ajw2003/PlunderSpell/issues/14) | No usable health or damage model | `gameplay` `enhancement` | [issues/014-no-health-damage-model.md](issues/014-no-health-damage-model.md) |
| [18](https://github.com/Ajw2003/PlunderSpell/issues/18) | No way to test combat | `gameplay` `enhancement` | [issues/018-no-combat-test-scene.md](issues/018-no-combat-test-scene.md) |
| [37](https://github.com/Ajw2003/PlunderSpell/issues/37) | No melee combat system | `gameplay` `combat` | [issues/037-no-melee-combat-system.md](issues/037-no-melee-combat-system.md) |
| [39](https://github.com/Ajw2003/PlunderSpell/issues/39) | Ranged weapons have no aim/fire | `gameplay` `enhancement` | [issues/039-ranged-weapons-no-aim-fire.md](issues/039-ranged-weapons-no-aim-fire.md) |
| [21](https://github.com/Ajw2003/PlunderSpell/issues/21) | No portal asset | `art` `gameplay` | [issues/021-no-portal-asset.md](issues/021-no-portal-asset.md) |
| [24](https://github.com/Ajw2003/PlunderSpell/issues/24) | EPIC: build the game loop end to end | `epic` `gameplay` | [issues/024-epic-full-game-loop.md](issues/024-epic-full-game-loop.md) |

## Phase 2 — Make it readable

**Why third:** once the loop can be won or lost, a player needs to *perceive* why. This is the
largest phase because "silent, invisible feedback" is the single most repeated finding across
both source audits.

| # | Title | Labels | Plan |
|---|---|---|---|
| [12](https://github.com/Ajw2003/PlunderSpell/issues/12) | No visual feedback for damage | `vfx` `enhancement` | [issues/012-no-damage-feedback.md](issues/012-no-damage-feedback.md) |
| [13](https://github.com/Ajw2003/PlunderSpell/issues/13) | No visual feedback for spells | `vfx` `enhancement` | [issues/013-no-spell-vfx.md](issues/013-no-spell-vfx.md) |
| [22](https://github.com/Ajw2003/PlunderSpell/issues/22) | No VFX or SFX anywhere | `vfx` `audio` | [issues/022-no-vfx-sfx-anywhere.md](issues/022-no-vfx-sfx-anywhere.md) |
| [10](https://github.com/Ajw2003/PlunderSpell/issues/10) | Enemies have no animations | `animation` `enhancement` | [issues/010-enemy-animations.md](issues/010-enemy-animations.md) |
| [11](https://github.com/Ajw2003/PlunderSpell/issues/11) | No player/door animations | `animation` `enhancement` | [issues/011-player-door-animations.md](issues/011-player-door-animations.md) |
| [47](https://github.com/Ajw2003/PlunderSpell/issues/47) | No loudness meter | `ui` `audio` | [issues/047-no-loudness-meter.md](issues/047-no-loudness-meter.md) |
| [49](https://github.com/Ajw2003/PlunderSpell/issues/49) | Recognised phrase has no caption | `ui` `accessibility` | [issues/049-no-recognised-phrase-caption.md](issues/049-no-recognised-phrase-caption.md) |
| [48](https://github.com/Ajw2003/PlunderSpell/issues/48) | Casting invisible to teammates | `networking` `vfx` `audio` | [issues/048-casting-invisible-to-teammates.md](issues/048-casting-invisible-to-teammates.md) |
| [46](https://github.com/Ajw2003/PlunderSpell/issues/46) | Portal has no closing-countdown feedback | `vfx` `audio` `gameplay` | [issues/046-portal-no-countdown-feedback.md](issues/046-portal-no-countdown-feedback.md) |
| [42](https://github.com/Ajw2003/PlunderSpell/issues/42) | Guards are silent | `audio` `enhancement` | [issues/042-guards-are-silent.md](issues/042-guards-are-silent.md) |
| [51](https://github.com/Ajw2003/PlunderSpell/issues/51) | No camera shake, hit-stop, or juice | `juice` `vfx` | [issues/051-no-camera-shake-hitstop.md](issues/051-no-camera-shake-hitstop.md) |
| [52](https://github.com/Ajw2003/PlunderSpell/issues/52) | Throwing/swinging have no wind-up weight | `animation` `juice` | [issues/052-no-throw-swing-weight.md](issues/052-no-throw-swing-weight.md) |

## Phase 3 — Measure the milestones

**Why fourth:** measuring now (not before Phase 2, not after Phase 5) means the loop is legible
enough that a failed measurement is informative rather than a re-discovery of Phase 0–2 gaps.

| # | Title | Labels | Plan |
|---|---|---|---|
| [50](https://github.com/Ajw2003/PlunderSpell/issues/50) | M1 acceptance never measured | `testing` `audio` | [issues/050-m1-acceptance-unmeasured.md](issues/050-m1-acceptance-unmeasured.md) |
| [55](https://github.com/Ajw2003/PlunderSpell/issues/55) | M2 acceptance never run | `testing` `gameplay` | [issues/055-m2-acceptance-unrun.md](issues/055-m2-acceptance-unrun.md) |
| [53](https://github.com/Ajw2003/PlunderSpell/issues/53) | No standalone player build | `testing` `build` | [issues/053-no-standalone-build.md](issues/053-no-standalone-build.md) |
| [54](https://github.com/Ajw2003/PlunderSpell/issues/54) | No performance budget/profiling | `testing` `performance` | [issues/054-no-performance-budget.md](issues/054-no-performance-budget.md) |
| [56](https://github.com/Ajw2003/PlunderSpell/issues/56) | Settings menu completeness unverified | `ui` `settings` | [issues/056-settings-menu-completeness.md](issues/056-settings-menu-completeness.md) |
| [57](https://github.com/Ajw2003/PlunderSpell/issues/57) | No keyboard-casting accessibility mode | `accessibility` | [issues/057-no-keyboard-casting-mode.md](issues/057-no-keyboard-casting-mode.md) |

## Phase 4 — Look and sound like the pitch

**Why fifth:** cosmetic/production-value work. Deliberately after the loop is provably playable
(Phase 0–3) so art isn't spent on a loop that might still change shape.

| # | Title | Labels | Plan |
|---|---|---|---|
| [16](https://github.com/Ajw2003/PlunderSpell/issues/16) | Main menu has no art | `art` `ui` | [issues/016-main-menu-no-art.md](issues/016-main-menu-no-art.md) |
| [23](https://github.com/Ajw2003/PlunderSpell/issues/23) | Castles look bland | `art` `enhancement` | [issues/023-castles-look-bland.md](issues/023-castles-look-bland.md) |
| [58](https://github.com/Ajw2003/PlunderSpell/issues/58) | Steam lobby has no art/branding | `ui` `art` `networking` | [issues/058-steam-lobby-no-art.md](issues/058-steam-lobby-no-art.md) |
| [59](https://github.com/Ajw2003/PlunderSpell/issues/59) | Six reference lighting frames missing | `art` `lighting` `design` | [issues/059-six-lighting-frames-missing.md](issues/059-six-lighting-frames-missing.md) |
| [34](https://github.com/Ajw2003/PlunderSpell/issues/34) | EPIC: no icon set exists | `epic` `art` `ui` | [issues/034-epic-no-icon-set.md](issues/034-epic-no-icon-set.md) |
| [40](https://github.com/Ajw2003/PlunderSpell/issues/40) | Weapon and loot icons don't exist | `art` `ui` | [issues/040-weapon-loot-icons.md](issues/040-weapon-loot-icons.md) |

## Phase 5 — Content decisions and missing pillars

**Why last (but not lowest priority):** #41 is flagged to decide **first within this phase** —
every hour spent animating/scoring the five divergent enemies before that decision is hours that
may need redoing. The Market and Lair epics are the biggest remaining pieces of net-new scope in
the project; both are pitch-critical but neither blocks a raid from being playable today.

| # | Title | Labels | Plan |
|---|---|---|---|
| [41](https://github.com/Ajw2003/PlunderSpell/issues/41) | Bestiary thematic divergence — **decide first** | `decision-needed` | [issues/041-bestiary-thematic-divergence.md](issues/041-bestiary-thematic-divergence.md) |
| [38](https://github.com/Ajw2003/PlunderSpell/issues/38) | Weapon roster doesn't match the pitch | `art` `content` `design` | [issues/038-weapon-roster-mismatch.md](issues/038-weapon-roster-mismatch.md) |
| [17](https://github.com/Ajw2003/PlunderSpell/issues/17) | Only one era has art and models | `art` `enhancement` | [issues/017-only-one-era-has-art.md](issues/017-only-one-era-has-art.md) |
| [35](https://github.com/Ajw2003/PlunderSpell/issues/35) | Era-specific hazards unimplemented | `gameplay` `content` `design` | [issues/035-era-specific-hazards.md](issues/035-era-specific-hazards.md) |
| [36](https://github.com/Ajw2003/PlunderSpell/issues/36) | Era-specific loot catalogue unauthored | `content` `art` `design` | [issues/036-era-specific-loot-catalogue.md](issues/036-era-specific-loot-catalogue.md) |
| [43](https://github.com/Ajw2003/PlunderSpell/issues/43) | GildedColossus boss has no behaviour | `gameplay` `design` | [issues/043-gildedcolossus-boss-behaviour.md](issues/043-gildedcolossus-boss-behaviour.md) |
| [44](https://github.com/Ajw2003/PlunderSpell/issues/44) | Door/socket types aren't functional hazards | `gameplay` `design` | [issues/044-door-socket-functional-hazards.md](issues/044-door-socket-functional-hazards.md) |
| [45](https://github.com/Ajw2003/PlunderSpell/issues/45) | Drawbridge has no operable mechanism | `gameplay` `animation` | [issues/045-drawbridge-no-mechanism.md](issues/045-drawbridge-no-mechanism.md) |
| [26](https://github.com/Ajw2003/PlunderSpell/issues/26) | EPIC: The Mystical Market pillar | `epic` `market` | [issues/026-epic-mystical-market.md](issues/026-epic-mystical-market.md) |
| [27](https://github.com/Ajw2003/PlunderSpell/issues/27) | Build the market hub | `market` `ui` | [issues/027-build-market-hub.md](issues/027-build-market-hub.md) |
| [28](https://github.com/Ajw2003/PlunderSpell/issues/28) | Author wares/pricing for all stalls | `market` `content` `design` | [issues/028-author-market-wares.md](issues/028-author-market-wares.md) |
| [29](https://github.com/Ajw2003/PlunderSpell/issues/29) | Market mark-up / leftover-spend rules | `market` `gameplay` | [issues/029-market-rules-unimplemented.md](issues/029-market-rules-unimplemented.md) |
| [30](https://github.com/Ajw2003/PlunderSpell/issues/30) | EPIC: The Lair is a menu, not a place | `epic` `lair` `art` | [issues/030-epic-lair-is-a-menu.md](issues/030-epic-lair-is-a-menu.md) |
| [31](https://github.com/Ajw2003/PlunderSpell/issues/31) | Build a 3D Lair space | `lair` `art` | [issues/031-build-3d-lair-space.md](issues/031-build-3d-lair-space.md) |
| [32](https://github.com/Ajw2003/PlunderSpell/issues/32) | Lair lighting and mood pass | `lair` `art` `lighting` | [issues/032-lair-lighting-mood-pass.md](issues/032-lair-lighting-mood-pass.md) |
| [33](https://github.com/Ajw2003/PlunderSpell/issues/33) | Debt/hoard have no in-fiction representation | `lair` `ui` | [issues/033-debt-hoard-no-in-fiction-representation.md](issues/033-debt-hoard-no-in-fiction-representation.md) |

## Status

| Phase | Issues | Status |
|---|---|---|
| 0 — Unblock the loop | 9 | Not started |
| 1 — Close the loop | 6 | Not started |
| 2 — Make it readable | 12 | Not started |
| 3 — Measure the milestones | 6 | Not started |
| 4 — Look/sound like the pitch | 6 | Not started |
| 5 — Content decisions + pillars | 16 | Not started |
| **Total** | **55** | |

Update the phase status rows as issues close. This document does not get rewritten per-issue —
only the status table and the "Dedup pass" date if a future re-triage happens.
