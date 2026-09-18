# Castle

Builds the fortress a raid happens in, from a single seed, identically on every machine. If this
is wrong, either the game can't agree on a floor plan across the network, or it traps a party
inside a building with no way out.

## What it owns

Deterministic layout generation, verifying that layout is walkable, replicating just the seed
across the network, and the door/lockdown state the alarm drives. It does not decide what loot or
guards go where (`raid.md` — `LootPlacementPlanner`/`GuardPlacementPlanner` consume the layout
this produces) and it does not decide when to escalate (`AlarmFSMManager`, see `alarm.md`) —
`CastleLockdown` only reacts to that state.

## How it works

- **`ProceduralCastleGenerator.Generate(seed)`** lays out concentric Chebyshev rings on a square
  grid, growing outward from a `CryptChamberFinal` at the origin through `Keep` → `InnerWard` →
  `OuterBailey` → `CurtainWall`. Every module attaches to an already-placed 4-neighbour, so the
  floor plan is guaranteed to be a single 4-connected region. A `System.Random` seeded once at the
  top drives every placement decision — never `UnityEngine.Random`, whose global static state is
  shared with VFX/audio and would make generation order-dependent (see `plunderspell.md` §7).
- **`CastlePathValidator.ValidatePath`** rasterises every placed module into a walkable grid at
  `CellScale`×`CellScale` resolution per layout cell (so adjacent modules' footprints touch and
  stay 4-connected) and runs a 4-directional A* from the crypt start to the extraction exit. This
  runs once at generation time, never per-frame.
- **`CastleNetworkManager`** is the only thing that crosses the network: the seed, as a PurrNet
  `SyncVar<int>`. Every peer's `OnSeedChanged` handler calls the same deterministic `Generate`
  locally — no mesh, module list, or transform is ever sent. If a layout fails validation, the
  server retries with `seed + 1` (not a fresh random number) up to `maxRetries`, so the seed it
  finally replicates is the one every client independently reproduces.
- **Doorways and door plugs.** Every enclosed room module is authored with an archway on all four
  sides (`_shell` in `Tools/AssetPipeline/castle_builders.py`). The generator places modules on the
  grid without consulting their geometry and rotates them to face their anchor, so a room with
  archways on only some sides would sooner or later meet its neighbour archway-to-blank-wall.
  Opening all four makes every 4-adjacency a real connection regardless of rotation. The cost is
  that a room on the outside of the layout has openings facing nothing, so
  `ProceduralCastleGenerator.SealOpenArchways` runs after placement and fills every archway whose
  4-neighbour cell is empty with that zone's door-plug prefab
  (`CastleRoomRegistry.GetDoorPlugForZone`, authored by `CastleDoorPlugForge`). There is one plug
  per enclosed zone because the archway size is derived from the zone's wall height — see
  `scale.md` ("Archways").
- **`CastleLockdown`** subscribes to `AlarmState` and locks (`Roused`) then bars (`HueAndCry`)
  every door — deliberately one-way, matching the alarm's own latch, so the castle can't hand back
  a mistake the players already paid for.

## Invariants

- **A raid never starts in a castle the crypt can't reach the exit from.** Generation is retried
  (seed walked forward, not re-rolled from scratch) until `CastlePathValidator` passes; see
  `raid.md`'s "A raid never starts in a castle you cannot walk out of."
- **Only the seed is ever sent over the network.** Generation must stay a pure function of it —
  any source of nondeterminism inside `Generate` (wall-clock time, `UnityEngine.Random`, iteration
  order over an unordered collection) breaks every client's ability to agree on the same building.
- **Lockdown only ever gets stricter.** `CastleLockdown` has no path back to unlocked; it mirrors
  `AlarmFSMManager`'s own latch at `Roused`.

- **Two 4-adjacent enclosed rooms are always door-connected, and no archway opens into empty
  space.** The first half comes from every room opening on all four sides, the second from
  `SealOpenArchways`. `CastleGeneratorTests.Test_AdjacentRoomsAreDoorConnected` holds both.
- **A module never leaves its grid cell.** `room_kit.FOOTPRINT` equals
  `ProceduralCastleGenerator.cellSize` (12 m) exactly, and `validate_in_blender` fails the asset
  build for any castle module whose XY bounding box reaches past ±6.05 m. Without that gate a
  module quietly grows into its neighbour's cell, which is what issue 19 was.

## Traps

- **The layout depends on whether a registry is assigned.** `PickWeighted` consumes a random draw
  when there is a room pool and returns early without one when there is not, so the same seed
  produces a different castle with and without prefabs. Compare two layouts only when both
  generators have the same registry.
- **`UnityEngine.Random` anywhere in the generation path is a silent multiplayer desync,** not a
  crash — clients drift apart with no error, because nothing here re-validates against a
  replicated layout, only against the seed. See `plunderspell.md` §7 for why this is called out as
  a hard constraint rather than a style preference.
- **A module's rasterised footprint must stay `CellScale`-aligned with its neighbours.** A room
  prefab whose collider doesn't match the grid footprint the generator assumed can pass placement
  but fail path validation (or the reverse), because the two use different representations of the
  same layout.
