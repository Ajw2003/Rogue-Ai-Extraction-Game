# [Issue #5] PCG: rooms do not connect and the floor plan does not read as a castle

**GitHub:** https://github.com/Ajw2003/PlunderSpell/issues/5
**Labels:** `pcg` `bug`
**Phase:** Phase 0 — Unblock the loop

**Blocks:** #25 (player spawn is currently a hardcoded world position; deriving it from the layout
only makes sense once the layout reliably reads as connected rooms), #6 (judging whether the
player/room scale is right requires rooms whose openings actually line up — right now you can't
tell whether a "too tall" reading is scale or a socket gap), #23 ("castles look bland" polish
in Phase 4 presumes a legible, connected floor plan to polish).
**Blocked by:** none — this is the first fix in the dependency graph for the castle generator.
Tracked and implemented together with #19 (see "Relationship to #19" below); neither is truly
separable from the other because both live in `ProceduralCastleGenerator.PlaceModule`/`PlaceRing`.

## Problem

The generated layout does not play as a castle. Rooms are placed on the grid but adjacent modules
do not line up into connected interior space, and the resulting flow (crypt outward to extraction)
is not legible or interesting to move through.

## Current state

`ProceduralCastleGenerator.Generate` (`Assets/_Project/Scripts/Runtime/Castle/ProceduralCastleGenerator.cs:69-102`)
lays out concentric Chebyshev rings from a crypt-final chamber at the origin outward through Keep →
InnerWard → OuterBailey → CurtainWall (`Rings`, lines 39-46), exactly as `docs/systems/castle.md:17-22`
describes. That part of the doc is accurate and current.

What the doc does not say, and what the issue is about, is *how* a module is attached to its
neighbour:

- `PlaceRing` (lines 110-163) walks ring-candidate cells, and for each unoccupied cell calls
  `TryFindPlacedNeighbour` (lines 319-343) to find *any* already-placed 4-neighbour. If one exists,
  the cell is accepted — there is no check of what socket either module exposes on the shared edge.
- The only thing derived from the neighbour is `facing` (line 146: `Vector2Int facing = neighbour - cell`),
  which feeds `RotationForFacing` (lines 356-362) to yaw the new module's local +Z toward the
  neighbour. This orients the module's *visual* front toward its anchor; it does not verify a door,
  window or archway is actually on that side.
- `InstantiateModule` (lines 186-213) instantiates the prefab, adds/finds `CastleRoomModule`, and
  calls `module.PopulateSockets()` (line 212) — populating `CastleRoomModule.Sockets` from the
  prefab's `SocketPoint` children. That is the *only* place sockets are touched during generation.

The socket system itself is fully built and unused:

- `SocketPoint.AreCompatible` / `AreTypesCompatible`
  (`Assets/_Project/Scripts/Runtime/Castle/SocketPoint.cs:92-117`) define which opening types may
  join (Door↔Door, Staircase↔Staircase, Window↔Window, ArchOpening↔ArchOpening,
  MurderHole↔WallSegment).
- `SocketPoint.Connect` (`SocketPoint.cs:120-128`) exists to pair two sockets and mark them occupied.
- `CastleRoomModule.FindFreeSocket` / `FreeSockets`
  (`Assets/_Project/Scripts/Runtime/Castle/CastleRoomModule.cs:61-81`) exist to query a module's
  open connectors.

A repo-wide search confirms `SocketPoint.Connect(...)` and `SocketPoint.AreCompatible(...)` are
never called anywhere outside their own declarations and one isolated unit test
(`CastleGeneratorTests.Test_SocketCompatibilityRules`,
`Assets/_Project/Scripts/Tests/Runtime/CastleGeneratorTests.cs:94-107`, which only exercises the
static type-compatibility table, not the generator). `CastleRoomModule.FindFreeSocket` has no
callers at all. The socket system is the same "authored but never wired in" pattern
`docs/systems/raid-scene-assembly.md:168-179` documents for the old placeholder era — except here
it was never wired in the first place, not regressed.

Measured directly from the prefabs (`Assets/_Project/Prefabs/Castle/*.prefab`, counting
`SocketPoint` component references by script GUID `644620259e38985d96708a3f28b07bf7`):

| Prefab | SocketPoint count |
|---|---|
| `WallStraight`, `WallCorner`, `Bastion` | **0** |
| `Drawbridge`, `GatehouseModule` | 1 |
| Most enclosed rooms (`BurialVault`, `ChapelRoom`, `StableBlock`, `StorehouseRoom`, `ThroneRoomKeep`, `TreasuryVault`, `LordsSolar`, `RoyalBedchamber`, `CryptAntechamber`) | 1 |
| `BarracksBunk`, `BlacksmithShop`, `CryptStairwell`, `GuardRoomInner`, `KeepStairwell`, `KitchenRoom`, `TombCorridor` | 2 |
| `ArmouredCourtyard`, `CryptChamberFinal`, `GreatHallMain`, `WellCourtyard` | 4 |

This confirms the issue's claim precisely: three CurtainWall modules carry zero sockets. Those
three are deliberately not enclosed rooms — `Tools/AssetPipeline/castle_builders.py:13-16` says so
directly ("The five CurtainWall modules are not enclosed rooms — they're the wall itself... so they
use room_kit's wall/tower/crenellation primitives directly instead of room_shell") — so a
socket-matching fix has to treat wall segments as a separate connection rule (edge-to-edge along the
ring), not force sockets onto them.

`CastlePathValidator.ValidatePath` (`Assets/_Project/Scripts/Runtime/Castle/CastlePathValidator.cs:30-84`)
only rasterises each module's grid *cell* into a `CellScale`×`CellScale` (2×2) walkable block and
runs A* over cell adjacency (lines 63-73). It has no knowledge of sockets, doors or mesh geometry at
all — it will report a path as long as two modules occupy 4-adjacent cells, which is exactly why the
generator can pass `CastlePathValidator` and `CastleGeneratorTests` today while producing rooms whose
door openings do not face each other.

## Root cause / gap analysis

The generator's connectivity guarantee operates entirely on the **abstract grid graph**
(`Dictionary<Vector2Int, int> occupied`), never on the **socket graph** the prefabs actually expose.
`TryFindPlacedNeighbour` only asks "is there an occupied cell next to me," never "does that occupied
module have a free, type-compatible socket facing me, and do I have one facing it." Because
`RotationForFacing` only yaws the module to look at its neighbour rather than to align a specific
socket with a specific socket, two adjacent rooms can each have a door on a side that is not the
shared edge (or no door on that edge at all, e.g. a room whose sole socket happens to be its north
wall placed south of its neighbour). The metadata graph (which `CastlePathValidator` walks) and the
mesh graph (what a player can actually walk through) are two different representations of "connected"
that nothing reconciles — this is exactly the trap `docs/systems/castle.md:53-56` warns about in the
abstract ("a room prefab whose collider doesn't match the grid footprint... can pass placement but
fail path validation, or the reverse") but the concrete failure here is socket-blind placement, not a
footprint/collider mismatch (that part is issue #19's territory).

## Relationship to #19

#19 ("Modules float and snap inconsistently") is the *spacing* half of the same generator:
`FOOTPRINT = 11.2` inside a `cellSize = 12` grid (`Tools/AssetPipeline/room_kit.py:24`,
`ProceduralCastleGenerator.cs:26`) leaves a systematic ~0.8 m gap at every join regardless of what
faces what. Fixing socket alignment (#5) while that gap remains would still leave every "connected"
pair of rooms floating apart by 0.8 m — a door facing a door across open air. Fixing the footprint
gap (#19) without socket alignment (#5) would leave rooms flush but with walls facing doors. **Both
must land together for either acceptance criterion to be independently verifiable**; see #19's plan
for the size/footprint half of this fix. This plan implements the socket-matching half and assumes
#19's grid/footprint fix lands in the same change (or immediately before it) so the two can be
verified against the same rebuilt castle.

## Implementation plan

1. **Add a deterministic socket-aware placement path to `ProceduralCastleGenerator`.**
   Replace the "any 4-neighbour" test in `TryFindPlacedNeighbour` with a socket-aware variant that
   also returns *which* socket pair to connect. This requires the candidate module's prefab to be
   inspected *before* final placement, which today only happens in `InstantiateModule` after the
   cell is already committed — so socket resolution has to move earlier, into `PlaceRing`.

   Add a new struct and a socket-resolving neighbour search to
   `Assets/_Project/Scripts/Runtime/Castle/ProceduralCastleGenerator.cs`:

   ```csharp
   /// <summary>A candidate connection: an occupied neighbour cell and the free socket on it
   /// available to join against.</summary>
   private readonly struct SocketLink
   {
       public readonly Vector2Int NeighbourCell;
       public readonly SocketPoint NeighbourSocket;

       public SocketLink(Vector2Int neighbourCell, SocketPoint neighbourSocket)
       {
           NeighbourCell = neighbourCell;
           NeighbourSocket = neighbourSocket;
       }
   }

   /// <summary>
   /// Finds a placed 4-neighbour of <paramref name="cell"/> that exposes a free socket compatible
   /// with at least one socket on <paramref name="candidatePrefab"/>. Wall-family modules (no
   /// sockets at all — WallStraight/WallCorner/Bastion) connect by cell-adjacency alone, matching
   /// how they are authored: solid curtain, not a room with a door.
   /// </summary>
   private bool TryFindSocketNeighbour(Dictionary<Vector2Int, int> occupied,
       List<GameObject> instantiatedByCell, Vector2Int cell, GameObject candidatePrefab,
       out SocketLink link)
   {
       link = default;
       int cellRing = Chebyshev(cell);
       SocketLink? fallback = null;

       foreach (Vector2Int d in FourDirs)
       {
           Vector2Int n = cell + d;
           if (!occupied.TryGetValue(n, out int neighbourIndex))
               continue;

           GameObject neighbourGo = FindInstantiatedAt(n);
           CastleRoomModule neighbourModule = neighbourGo != null
               ? neighbourGo.GetComponent<CastleRoomModule>() : null;

           bool isInward = Chebyshev(n) < cellRing;

           // A wall-family module (no sockets on either side) still connects by adjacency.
           if (neighbourModule == null || neighbourModule.Sockets.Count == 0 ||
               !HasAnySocket(candidatePrefab))
           {
               var adjacencyLink = new SocketLink(n, null);
               if (isInward) { link = adjacencyLink; return true; }
               fallback ??= adjacencyLink;
               continue;
           }

           foreach (SocketPoint candidateSocket in candidatePrefab.GetComponentsInChildren<SocketPoint>(true))
           {
               SocketPoint freeNeighbourSocket = FindCompatibleFreeSocket(neighbourModule, candidateSocket.Type);
               if (freeNeighbourSocket == null)
                   continue;

               var socketLink = new SocketLink(n, freeNeighbourSocket);
               if (isInward) { link = socketLink; return true; }
               fallback ??= socketLink;
           }
       }

       if (fallback.HasValue) { link = fallback.Value; return true; }
       return false;
   }

   private static SocketPoint FindCompatibleFreeSocket(CastleRoomModule module, SocketType type)
   {
       foreach (SocketPoint s in module.FreeSockets())
           if (SocketPoint.AreTypesCompatible(s.Type, type))
               return s;
       return null;
   }

   private static bool HasAnySocket(GameObject prefab) =>
       prefab.GetComponentInChildren<SocketPoint>(true) != null;
   ```

2. **Rewrite `PlaceRing` to only accept a cell when a socket link (or wall-adjacency fallback)
   exists**, and thread the matched socket through to `PlaceModule`/`InstantiateModule` so the two
   sockets get `SocketPoint.Connect`ed after instantiation:

   ```csharp
   // inside the candidate loop in PlaceRing, replacing the current
   // "if (!TryFindPlacedNeighbour(...)) continue;" block:
   string roomId = PickWeighted(weightedPool, rng, ring.Zone);
   CastleRoomModuleData entry = registry?.GetById(roomId);
   GameObject candidatePrefab = entry?.Prefab;

   if (candidatePrefab != null)
   {
       if (!TryFindSocketNeighbour(occupied, _instantiated, cell, candidatePrefab, out SocketLink socketLink))
           continue; // no compatible opening anywhere on this cell's placed neighbours

       Vector2Int facing = socketLink.NeighbourCell - cell;
       int newIndex = PlaceModule(data, occupied, roomId, ring.Zone, cell, facing, isCryptEntry: false);
       ConnectSockets(newIndex, socketLink);
   }
   else
   {
       // data-only layout (no prefab assigned yet): fall back to plain adjacency, as today.
       if (!TryFindPlacedNeighbour(occupied, cell, out Vector2Int neighbour))
           continue;
       Vector2Int facing = neighbour - cell;
       PlaceModule(data, occupied, roomId, ring.Zone, cell, facing, isCryptEntry: false);
   }
   ```

   `ConnectSockets` resolves the newly-instantiated module's matching socket (the one whose type is
   compatible with `socketLink.NeighbourSocket.Type`, or nearest to the shared edge when both sides
   are wall-family) and calls `SocketPoint.Connect(newSocket, socketLink.NeighbourSocket)`, finally
   marking both `IsOccupied` so a later ring can't reuse them.

3. **Test first (EditMode).** Add to
   `Assets/_Project/Scripts/Tests/Runtime/CastleGeneratorTests.cs`:

   ```csharp
   [Test]
   public void Test_PlacedModulesConnectThroughCompatibleSockets()
   {
       // Requires a registry with real prefabs wired (this test intentionally exercises
       // InstantiateModule, not just the metadata layout — see "Testing & verification plan").
       var gen = MakeGenerator();
       gen.Registry = TestCastleRegistry.LoadRealRegistry(); // helper: loads the committed
                                                              // CastleRoomRegistry.asset by path
       ProceduralCastleData data = gen.Generate(42);

       int connectedPairs = 0;
       foreach (GameObject go in gen.LastInstantiatedForTest)
       {
           var module = go.GetComponent<CastleRoomModule>();
           foreach (SocketPoint s in module.Sockets)
           {
               if (s.ConnectedTo == null)
                   continue;
               Assert.IsTrue(SocketPoint.AreTypesCompatible(s.Type, s.ConnectedTo.Type),
                   $"{module.RoomId} socket {s.Type} connected to incompatible {s.ConnectedTo.Type}");
               connectedPairs++;
           }
       }
       Assert.Greater(connectedPairs, 0, "At least one socket pair must have been connected.");
   }
   ```

   This asserts the *rule*, not a specific seed's exact layout, matching the project's existing
   "assert the rule, not the eyeball" pattern noted in `docs/systems/raid.md:27-30`. Write this test
   before step 1-2's implementation exists (it will fail against the current generator, since no
   `SocketPoint.Connect` is ever called), then make it pass.

4. **Give the three sockets-less CurtainWall modules an explicit adjacency contract.** Since
   `WallStraight`/`WallCorner`/`Bastion` are wall segments by design (not rooms), don't force
   `SocketPoint`s onto them — instead codify in a comment on `TryFindSocketNeighbour` (already
   included in step 1's sketch) that a module with zero sockets connects by cell-adjacency to
   anything, exactly like today, and is excluded from the "door must face a door" guarantee. This
   keeps `docs/systems/castle.md`'s "every module attaches to an already-placed 4-neighbour" true
   while making it additionally true, for every *enclosed room*, that the neighbour is attached
   through a real opening.

5. **Update `docs/systems/castle.md`** ("How it works" bullet on `ProceduralCastleGenerator.Generate`,
   lines 17-22) to describe socket-matching once implemented, since the doc currently states only the
   grid-adjacency guarantee and would otherwise go stale the moment this ships.

## Testing & verification plan

- **EditMode:** extend `Assets/_Project/Scripts/Tests/Runtime/CastleGeneratorTests.cs` (namespace
  `RogueAi.Tests`, assembly `RogueAi.Tests` —
  `Assets/_Project/Scripts/Tests/Runtime/RogueAi.Tests.asmdef`, which has empty `includePlatforms`
  so it runs under both the EditMode and PlayMode test runners; these particular tests use no scene
  and no `[UnityTest]` coroutine, so they run as EditMode tests in practice) with:
  - `Test_PlacedModulesConnectThroughCompatibleSockets` (step 3 above), run across a fixed set of
    seeds (1–20, matching the existing convention in `RaidLoopTests.Test_TheCryptAlwaysHoldsItsRelic`)
    to catch a seed-dependent regression rather than one lucky seed.
  - `Test_WallFamilyModulesStillConnectByAdjacency` — generates a castle, asserts every
    `WallStraight`/`WallCorner`/`Bastion` instance has a 4-adjacent placed neighbour (the
    pre-existing guarantee), confirming step 4 didn't break the CurtainWall ring.
  - Re-run `Test_DeterministicGeneration`, `Test_AllZonesPresent`, `Test_PathValidatorFindsPath`,
    `Test_ExtractionExitAssigned` unmodified — these must keep passing, since the grid-graph
    guarantee (`docs/systems/castle.md`'s existing invariant) is not being removed, only
    strengthened.
- **PlayMode / manual:** `CastlePathValidator` cannot see meshes, so the only way to actually confirm
  "a player can walk from the crypt chamber to the extraction exit entirely through interior space"
  is to drive a real Play session. Protocol: run `Tools/Plunderspell/Build Playable Raid Scene`,
  enter Play, use `RaidBootstrapper`'s F5/F6 (`docs/systems/raid.md:33`) to start several raids across
  different seeds, and walk (not noclip) from the crypt chamber to the OuterBailey extraction module
  for each. Pass/fail: the walk never requires clipping through a wall, falling off the world, or
  passing over a gap; every doorway crossed is a rendered opening, not a wall face. Do this for at
  least 5 distinct seeds since placement is seed-dependent. No confirmed Unity Editor install is
  verified in this environment (see "Effort & risk"), so this step cannot be executed as part of this
  plan — it is the acceptance gate a human (or a driven-Editor CLI session per
  `docs/systems/raid-scene-assembly.md`'s "Verification" section) must run before closing the issue.

## Acceptance criteria

- [ ] Adjacent placed modules share a wall or a doorway; no unintended gap between neighbours.
- [ ] Socket matching (or an equivalent rule) drives placement so an opening always faces an opening.
- [ ] A player can walk from the crypt chamber to the extraction exit entirely through interior space.
- [ ] The existing `CastleGeneratorTests` determinism and reachability tests still pass.

## Visual

```mermaid
flowchart TD
    subgraph Today["Current placement (ProceduralCastleGenerator.PlaceRing)"]
        A1[Candidate cell] --> A2{Any 4-neighbour\noccupied?}
        A2 -- no --> A3[Skip cell]
        A2 -- yes --> A4[Place module,\nyaw to face neighbour]
        A4 --> A5["PopulateSockets()\n(sockets recorded, never matched)"]
        A5 --> A6["CastlePathValidator: cell-adjacent\n=> reports 'connected'"]
        A6 --> A7["Mesh reality: door may face\na wall, or nothing (0.8m gap, see #19)"]
    end

    subgraph Fixed["Fixed placement (this plan)"]
        B1[Candidate cell] --> B2{TryFindSocketNeighbour:\ncompatible free socket\non a placed neighbour?}
        B2 -- no --> B3[Skip cell]
        B2 -- yes, wall-family --> B4[Place by adjacency\n(unchanged, by design)]
        B2 -- yes, socketed --> B5[Place + yaw to align\nthe matched socket pair]
        B5 --> B6["SocketPoint.Connect(new, neighbour)\nboth marked occupied"]
        B4 --> B7[CastlePathValidator: unchanged,\nstill cell-adjacent]
        B6 --> B7
        B7 --> B8["Mesh reality: every enclosed-room\nconnection has door facing door"]
    end

    style A7 fill:#5a2020,color:#fff
    style B8 fill:#1f5a2e,color:#fff
```

## Effort & risk

**Size: L.** The algorithmic change (socket-aware neighbour search) is moderate, but it touches the
core placement loop that every other castle behaviour depends on (`CastlePathValidator`,
`GuardPlacementPlanner`, `LootPlacementPlanner` all consume `ProceduralCastleData` this produces), so
regressions here are wide-blast-radius.

Risks:
- **Ring-fill starvation.** Requiring a *compatible* socket (not just an occupied neighbour cell) is
  strictly more restrictive than today's rule, so some rings may no longer be able to place their
  full `MinCount..MaxCount` range for some seeds — a room with a Staircase-only socket has nowhere to
  attach next to a Door-only room. This needs either (a) authoring more socket-type variety per zone,
  or (b) a documented fallback (e.g. allow a "forced" wall-adjacency placement, logged as a warning,
  when no seed retry within `RaidDirector.GenerateWalkable`'s `maxAttempts = 16`
  (`Assets/_Project/Scripts/Runtime/Raid/RaidDirector.cs:301`) finds a fully socket-satisfied layout).
  Decide which before implementing step 2, since it changes the function signature.
- **Determinism.** `TryFindSocketNeighbour`'s neighbour/socket iteration order must be as stable as
  `TryFindPlacedNeighbour`'s (`FourDirs` iterates in a fixed array order) — introducing a `Dictionary`
  or unordered enumeration anywhere here would violate `docs/systems/castle.md`'s "only the seed is
  ever sent over the network" invariant silently, per the "traps" section
  (`docs/systems/castle.md:49-52`).
- **No confirmed Unity Editor install in this working environment.** The manual PlayMode walk-through
  in "Testing & verification plan" and the compile-correctness of the code sketches above must be
  verified against the real Unity 6000.3.15f1 toolchain before this is called done — the sketches
  here are written to the project's conventions and existing APIs but have not been compiled.
- Overlaps functionally with #19 (see "Relationship to #19"); implementing this without #19's
  footprint fix landing alongside it will pass this issue's tests while still failing #19's, and the
  reverse.

## Open questions

- Should `TryFindSocketNeighbour`'s "no compatible socket anywhere" case fail the whole ring (forcing
  a seed retry via `RaidDirector.GenerateWalkable`) or fall back to a forced wall-style join with a
  logged warning? This changes both the function contract in step 1 and the "ring-fill starvation"
  risk above — needs a decision before implementation, not during code review.
- Do any of the 25 authored room prefabs need *additional* `SocketPoint`s added (content work, not
  code) to give every zone enough socket-type variety to avoid starvation? That can only be answered
  empirically by running the new generator against the real registry across many seeds.

## Definition of done

A reviewer regenerates the raid scene, walks the crypt-to-extraction path in Play mode across at
least 5 seeds without clipping through geometry or crossing an unopened wall, and confirms
`CastleGeneratorTests` (including the new socket-connectivity test) and `RaidLoopTests` all pass.
