# Scale

The metre. Every room, doorway, player and enemy in Plunderspell is sized against one standard
figure, and this is where that figure is written down. If this is wrong, rooms feel like
doll's houses or cathedrals, enemies clip through ceilings, and a boss spawns somewhere it can
never move.

## What it owns

The one number the whole game is measured against, the per-zone room heights derived from it, the
archway sizes derived from those, and the enemy standing heights checked against them. It does not
own how a room is laid out (`castle.md`) or how the scene is assembled (`raid-scene-assembly.md`)
— it owns only the sizes those two must agree on.

## The standard

**A standard human is 1.80 m tall**, with eyes at **1.65 m** and a body radius of **0.40 m**.

That is the player (`RaidSceneBuilder.PlayerHeight` / `EyeHeight` / `PlayerRadius`) and it is the
`Watchman`, the plainest guard in the roster. Everything else is described as a multiple or
fraction of it rather than as an independent number.

## Rooms

Castle modules are authored in `Tools/AssetPipeline/castle_builders.py`. A module is a **12.0 m ×
12.0 m** footprint — exactly `ProceduralCastleGenerator.cellSize`, so placed modules touch with no
gap — carrying a **0.30 m** floor slab with walls rising from the top of that slab.

`ZONE_HEIGHT` is that wall height, i.e. the clear height above the floor:

| Zone | Wall height above the floor | Total from ground | Headroom over a 1.80 m human |
|---|---|---|---|
| Crypt | 3.00 m | 3.30 m | 1.20 m |
| OuterBailey | 3.60 m | 3.90 m | 1.80 m |
| InnerWard | 4.00 m | 4.30 m | 2.20 m |
| Keep | 4.60 m | 4.90 m | 2.80 m |
| CurtainWall | 5.20 m | 5.50 m | — (not an enclosed room) |

The ring grows grander outward from the Crypt, which is the tightest room in the game by design.

## Archways

Every enclosed room opens on all four sides. The opening is derived from the zone's wall height by
`room_kit.opening_size()`: **2.60 m wide**, and `min(height × 0.72, wallHeight − trim − 0.25)`
tall. That yields:

| Zone | Archway (w × h) | Headroom over a 1.80 m human |
|---|---|---|
| Crypt | 2.60 × 2.16 m | 0.36 m |
| OuterBailey | 2.60 × 2.59 m | 0.79 m |
| InnerWard | 2.60 × 2.88 m | 1.08 m |
| Keep | 2.60 × 3.31 m | 1.51 m |

The tightest archway in the game therefore clears a standard human by more than a third of a metre
and is wide enough for two abreast.

## Enemies

`EnemyPrefabForge.Specs` carries a standing height per enemy, and the forge scales each model
uniformly from its authored height to it. The authored heights in
`Assets/Models/Enemies/enemy_manifest.json` disagreed with each other by a factor of four; this is
what reconciles them.

| Enemy | Standing height | Zones |
|---|---|---|
| WarHound | 0.85 m | OuterBailey, InnerWard |
| SigilWisp | 1.20 m | InnerWard, Keep |
| HexTurret | 1.60 m | CurtainWall, Keep |
| CryptRisen | 1.75 m | Crypt |
| Watchman | 1.80 m | CurtainWall, OuterBailey |
| ManAtArms | 1.85 m | OuterBailey, InnerWard |
| Sergeant | 1.90 m | InnerWard, Keep |
| VaultWarden | 2.10 m | Keep, Crypt |
| ArcRevenant | 2.10 m | Keep, Crypt |
| GildedColossus | 2.50 m | Crypt |

The `GildedColossus` is the ceiling case. It is posted to the Crypt, the shortest zone, so its
2.50 m is set against the Crypt's 3.00 m clear height — head and shoulders over any guard, half a
metre of air above it, and no clipping through the room it fights in.

## Invariants

- **A standard human is 1.80 m.** Any new character height is stated relative to that figure, and
  the roster table above is the full list — an enemy without an entry has not been scaled.
- **No enemy is taller than the clear height of the shortest zone it is posted to.** The
  `GildedColossus` at 2.50 m against the Crypt's 3.00 m is the tightest pair in the game.
- **Every archway clears 1.80 m.** The Crypt's 2.16 m opening is the smallest; anything that
  lowers `ZONE_HEIGHT["Crypt"]` below 2.50 m breaks this.
- **Floors sit at ground level in every zone.** A taller zone is taller at the ceiling, never
  lower at the floor, so a player crossing a zone boundary never steps up or down.

## Verification

`ScaleInvariantTests` (EditMode) measures the shipped art against this document rather than
restating its numbers: it instantiates each prefab and reads real world-space renderer bounds, so a
mesh that disagrees with the table fails rather than passing on a copied constant.

| Test | Asserts |
|---|---|
| `Test_ThePlayerCapsuleIsTheStandardHuman` | `CastleSpawnResolver` is sized 1.80 m × 0.40 m |
| `Test_EveryRoomModuleClearsAStandardHuman` | every registry room's clear height exceeds 1.80 m |
| `Test_NoEnemyIsTallerThanTheRoomsItIsPostedTo` | no enemy exceeds the shortest zone's clear height |
| `Test_EveryEnemyStandsOnItsOwnOrigin` | feet at the origin — **currently `[Ignore]`d, see issue 94** |

Archway clearance is **not** covered: the opening is a hole in a mesh rather than an object, so
nothing here measures it. The archway figures above are still derived from
`Tools/AssetPipeline/room_kit.opening_size()`, not verified against the art.

## Traps

- **Three enemies do not stand on the floor.** `ArcRevenant` floats 0.15 m, `GildedColossus`
  sinks 0.16 m and `VaultWarden` sinks 0.12 m, because `EnemyPrefabForge` assumes the source models
  are exported feet-on-origin and only scales them. Issue 94.
- **The `GildedColossus` fits in the Crypt but not through its archway.** At 2.50 m tall and
  1.57 m wide it cannot pass a 2.60 × 2.16 m opening standing up. It is a room-bound boss: it
  fights where it spawns. Anything that expects it to patrol between rooms needs either a taller
  Crypt archway or a shorter Colossus, and changing one without the other is the bug.
- **A collider's numbers are local; a NavMeshAgent's are world.** `EnemyPrefabForge` scales the
  model's transform, so `CapsuleCollider.height` takes the *unscaled* measurement and
  `NavMeshAgent.height` takes the scaled one. Setting both to the same number makes the agent and
  its body disagree by the scale factor.
- **The floor's top face is a couple of millimetres above 0.30 m.** `room_kit` grows every stacked
  box by `OVERLAP` so flush joints do not z-fight, which puts the walkable surface at ~0.31 m. A
  capsule placed to rest at exactly 0.30 m intersects the slab, and every overlap probe against it
  reads as blocked — see `RaidSceneBuilder.FloorClearance`.

## Spawning

`CastleSpawnResolver.ResolveSpawn(layout)` is the single derivation, shared by the runtime and the
editor. It takes the extraction-exit module — the gatehouse, the castle's one gate — steps 3.5 m
inward from its centre (the gatehouse's wall, portcullis bars and flanking drum towers all crowd
the outward half of that cell) and probes for the first player-sized capsule that touches no
collider: the anchor first, then two rings of eight.

Two callers, one probe:

- **`RaidDirector.PlacePlayerAtSpawn`** runs on every `BuildCastle`, so the spawn follows whatever
  seed the raid actually rolled. This is the one that matters at play time.
- **`RaidSceneBuilder`** bakes a spawn for `ProceduralCastleGenerator.defaultSeed` into
  `RaidScene.unity`, so the authored scene looks right when opened. The director overwrites it at
  raid start.

**Trap.** `Physics.SyncTransforms()` must run between instantiating the castle and probing it, or
the colliders are still at their previous transforms and every candidate reads as clear. Both
callers do this.
