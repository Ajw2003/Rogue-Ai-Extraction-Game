# Plan: Issue 20 - Loot Physics Explosion on Spawn

## Exhaustive Outline
Currently, when the game generates a castle and spawns loot, many items are placed slightly inside walls or furniture. Because the items use a physics system, the game immediately tries to push them out of the solid objects, causing the loot to violently launch across the room, into the ceiling, or out of the game world entirely. The goal of this task is to stop this explosion. We will achieve this by spawning the loot in a frozen state, waiting briefly for the game world to finish loading, and then turning physics on so the loot drops gently to the floor.

## Step by Step Execution Instructions

1.  **Spawn Loot as Kinematic (Frozen):**
    Open the `LootSpawner` script. When creating a new piece of loot, immediately set its Rigidbody component to be "Kinematic". A kinematic object is ignored by physics forces, so it will not explode away if it overlaps a wall.

2.  **Wait for the Scene to Settle:**
    Create a coroutine or timer in the `LootSpawner` that waits for a short period, such as 0.5 seconds, after all rooms and objects have finished spawning.

3.  **Unfreeze the Loot:**
    After the short wait, loop through all the newly spawned loot items. Check that they are not currently being carried by a player. If they are free, turn off the Kinematic setting on their Rigidbody. This hands them back to the physics engine, allowing them to gently fall to the floor.

4.  **Preserve Determinism Rules:**
    Ensure these changes only happen in the `LootSpawner` script. Do not modify the `LootPlacementPlanner`, as it must remain a pure mathematical calculation so all players in a multiplayer game generate the exact same loot locations.

## Verification Steps
1.  Launch the game and start a new raid.
2.  Open the developer console or Unity inspector and check the Y-coordinates of all spawned loot.
3.  Verify that zero items are falling infinitely below the map or stuck high in the sky.
4.  Walk through the castle rooms and look for the loot.
5.  Verify the loot is resting peacefully on the floor or on furniture, and is not bouncing around wildly.
6.  Run the automated `RaidLoopTests` to verify the deterministic generation still works perfectly.

## Completion Checks
*   [ ] Spawned loot no longer flies out of the level.
*   [ ] Loot rests visibly on surfaces inside the correct rooms.
*   [ ] The mathematical `LootPlacementPlanner` remains unchanged and deterministic.
*   [ ] The automated determinism tests still pass successfully.


## Technical Constraints
When executing this plan, you MUST read and strictly adhere to ALL principles and conventions detailed in `docs/UnityConvention.md`. 
You cannot pick and choose which rules to enforce; every single rule applies.
Specifically, you must follow:
- Core Architectural Principles: KISS, YAGNI, Solve the Root Cause, DRY, and SRP.
- All Naming Conventions (e.g., `m_camelCase` for privates, `s_camelCase` for statics, `PascalCase` for methods/properties).
- All Formatting & Syntax rules (e.g., Allman braces, mandatory braces, 4-space indentation).
- Class & Method Organization (Newspaper metaphor, correct layout order).
- Unity-Specific Implementations (e.g., `[SerializeField]` instead of public, `[Tooltip]` instead of comments).
- UI Toolkit (UXML/USS) Naming (BEM convention, kebab-case).
- Commenting rules (Explain 'Why', not 'What').
