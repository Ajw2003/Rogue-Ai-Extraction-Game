# Decisions

Append-only. An entry is never rewritten or deleted; the one allowed edit is flipping its
`Status` line to `Superseded` when a later entry replaces it. Newest entry at the top.

## 2026-09-16 — Complete the docs structure by salvaging an abandoned scaffold branch, not rewriting it

**Context.** Running a `docs/` structure/audit pass, `origin/claude/repo-status-check-hjp6z7` was
found: a fully-written five-tier scaffold (tiers 1, 2, 3, 5, plus `docs/systems/README.md` and
three previously-missing system docs — `voice.md`, `castle.md`, `alarm.md` — and the
`docs/archive/2026-09-15-integration/` move) committed once (`d721d7b`) and never merged. It was
cut from a point 11 commits behind current `main`, so its tier 1/2/3/5 drafts were stale relative
to the raid-scene-assembly work, the menu/lair navigation feature, and the 2026-09-16 issue
backlog that landed afterward — but its tier-4 system docs (voice, castle, alarm) covered code
that hadn't changed at all in the interim.

**Decision.** Adopt the branch's `voice.md`, `castle.md` and `alarm.md` verbatim; rewrite
`README.md`, `Roadmap.md`, `ProjectState.md` and `Today.md` using its structure and reasoning as a
base, updated for what shipped since; add the tier 6 (`Decisions.md`) and `docs/generated/` it
didn't have; and additionally move the still-loose HTML previews and the live GitHub issue
manifest into `docs/generated/`, which the branch had left at the `docs/` root.

**Why.** The branch's tier-4 work was accurate, well-cited, and covered systems (voice, castle,
alarm) that no other doc addressed — discarding it and re-deriving the same analysis from scratch
would have cost real effort for no better result. Its tier 1/2/3/5 drafts needed updating either
way, since a project's current state is exactly the part of this structure that's expected to move
between passes.

**Status.** Standing.

## 2026-09-16 — `RaidSceneBuilder` aborts when a catalogue is missing, rather than falling back to primitives

**Context.** The scene builder used to generate its own placeholder box room, gold cube and
capsule guard whenever the room/loot/enemy catalogues were empty. Meanwhile the project already
contained 25 modelled castle rooms, 5 modelled loot prefabs and 10 rigged enemy models, wired into
complete `ScriptableObject` catalogues — none of it referenced by anything. The placeholder
fallback was silent, so this went unnoticed for the length of an entire asset-pipeline effort. See
`docs/systems/raid-scene-assembly.md` ("The placeholder era").

**Decision.** `RaidSceneBuilder` now aborts the build and names every missing catalogue by name,
instead of silently substituting primitives.

**Why.** A silent fallback is precisely what let real, already-finished art sit unused — a loud
failure at build time makes a missing catalogue impossible to miss again.

**Status.** Standing.

## 2026-09-16 — Compose spawn rotations with the prefab's own rotation; never replace it

**Context.** Castle room, loot and enemy prefabs each carry a different Blender-to-Unity axis
correction baked into their root rotation (see `docs/systems/raid-scene-assembly.md`,
"Orientation"). `RaidSceneBuilder` and `ProceduralCastleGenerator.PlaceModule` called
`Instantiate(prefab, pos, rot, parent)`, which overwrites a prefab's root rotation outright —
laying every castle room on its edge.

**Decision.** Every spawn site composes instead: `Instantiate(prefab, pos, rot *
prefab.transform.rotation, parent)`.

**Why.** The three prefab families don't agree on where their axis correction lives, and fixing
that inconsistency would mean re-authoring art. Composing rather than replacing respects whatever
convention each family already uses, and is the minimal code-only fix.

**Status.** Standing.

## 2026-09-15 — Loot placement stays a pure function; the physics-fling defect is not patched by raycasting

**Context.** `LootPlacementPlanner` places loot 0.5 m above a room's centre as a pure function of
(layout, table, seed) — no scene, no components, no time. Against real room geometry, this can
land inside a wall or prop, and PhysX ejects the overlapping rigidbody hard: measured at roughly
2–7 of ~19 pieces flung per raid. An attempt to fix this by raycasting downward for the floor made
it measurably worse (15/22 flung), because loot ended up landing on room roofs instead.

**Decision.** Revert the raycast attempt. Leave the fling defect open and documented
(`docs/systems/raid-scene-assembly.md`, "Traps"; tracked as GitHub issue #20) rather than
compromise the planner's purity for a fix that didn't work.

**Why.** The planner being pure — knowing the layout and the seed but nothing about mesh geometry
— is what makes placement *rules* assertable in a test rather than eyeballed in the editor. A real
fix needs the *spawner* (which does touch the scene) to find a clear resting spot, not the planner
to stop being pure.

**Status.** Standing — open defect.

## 2026-09-14 — Authority checks read `isSpawned && !isServer`, never `isServer` alone

**Context.** PurrNet's `isServer` is false both on a real client and on an object that was never
spawned onto the network at all. `if (!isServer) return;` therefore silently disables a system in
single-player, because an unspawned object looks identical to a client to that check. This caused
three separate, independently-discovered silent failures: voice casting (the entire game did
nothing offline because `OnSpawned` never fired, so nothing ever subscribed), the extraction
clock (never counted down), and trigger tracking.

**Decision.** Every authority check in the project reads `if (isSpawned && !isServer) return;` —
an unspawned object is treated as its own authority.

**Why.** This is the one check that's correct in both single-player (unspawned) and real
multiplayer (spawned client), without a separate offline code path. See `docs/systems/raid.md`,
`docs/systems/voice.md` and `docs/systems/alarm.md` for where this bit.

**Status.** Standing — the underlying trap is a property of PurrNet's authority model, not
something the codebase can rule out for a future `NetworkBehaviour` that skips the `isSpawned`
half.

## 2026-09-11 — `feature/Owen/PCG` is a clean-room boundary

**Context.** A branch survey for the Plunderspell pivot found `feature/Owen/PCG`, 9 commits of
procedural room generation by a contributor who has since left the project. Before the
restriction below was decided, `ProceduralRoom.cs` (~78 lines) was read in full, along with that
branch's file list, as part of the same survey.

**Decision.** The castle generator is written from the pitch/plan specification and general
castle architecture only. `feature/Owen/PCG` is never opened, diffed, merged, cherry-picked or
rebased into this lineage again. The one file already read is recorded rather than treated as
unread: it implements a recursive branching room-web (random rotation, retry-on-overlap), which is
a different algorithm from the deterministic outward-in ward nesting this project's generator
uses (`docs/systems/castle.md`).

**Why.** Avoids any dependency — even a convergent, coincidental one — on code from a departed
contributor whose branch was never a sanctioned reference for this codebase.

**Status.** Standing.

## 2026-09-11 — Fork the wizard pivot from `claude/steam-multiplayer-framework-xia7ch`, not `main`

**Context.** A branch survey ahead of the Plunderspell pivot found the branch that was checked
out at the time was a stale copy of `main`, 30 commits behind. `claude/steam-multiplayer-framework-xia7ch`
already contained a working first-person Rigidbody FSM, physics grab/carry/throw, a working
`SpellBook`, Steam multiplayer via PurrNet, and a clean `Core` module — roughly 2,900 lines beyond
`main`.

**Decision.** Reset the working branch onto that trunk (`git checkout -B <branch>
origin/claude/steam-multiplayer-framework-xia7ch`) rather than building the pivot from `main` or
continuing on the stale branch.

**Why.** Building from `main` would have meant re-deriving nearly 3,000 lines of already-working
FSM, physics and networking code from scratch for no benefit.

**Status.** Standing.

## 2026-09-11 — Voice casting is on-device keyword spotting; no audio ever leaves the machine

**Context.** Decided with the user while scoping the voice-casting pillar of the Plunderspell
pivot (`docs/plans/plunderspell.md`, "Decisions taken with the user").

**Decision.** Recognition runs entirely on the player's own machine (Vosk, with a keyboard mock
where no microphone is available). Only the recognised spell id crosses the network, as a PurrNet
RPC — never raw audio.

**Why.** Stated as a design pillar, not just an implementation detail: no server cost per player,
no latency floor from a network round trip, and no voice data ever leaves the house
(`docs/plunderspell.md` §4).

**Status.** Standing.

## 2025-09-17 — `SingletonBase.Instance` never auto-creates a scene owner

**Context.** The originally ported `SingletonBase.Instance` auto-created a `GameObject` when
`_instance` was `null`, which meant `EventManager.Instance?.Publish(...)` could never actually
observe a missing instance, and spawned a stray object after scene teardown. It also always called
`DontDestroyOnLoad`, ignoring the `PersistBetweenScenes` flag's own stated purpose.

**Decision.** The getter returns `null` when no instance has claimed ownership, instead of
self-creating one; `DontDestroyOnLoad` is only called when `PersistBetweenScenes` is true.

**Why.** An absent instance is information a caller needs (no scene owner has been set up yet),
not a problem to paper over by spawning a `GameObject` nobody asked for. See
`docs/systems/core.md`'s invariant: "`SingletonBase.Instance` never creates anything."

**Status.** Standing.
