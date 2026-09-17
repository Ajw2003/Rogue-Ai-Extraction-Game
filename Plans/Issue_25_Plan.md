# Plan: Issue 25 - Player Spawn Position Fix

## Exhaustive Outline
The player currently spawns at a fixed position, which can cause them to start inside solid objects or walls created by the level generator. The goal is to update the spawning logic so the player always starts in open, walkable space based on the generated layout, avoiding any overlaps with solid structures.

## Step by Step Execution Instructions

1.  **Analyze Current Spawning Logic:**
    Review how the player is placed in the level builder script and identify the layout data available at the time of spawning.
2.  **Determine Valid Spawn Locations:**
    Update the level generation process to find a safe, open coordinate (such as near the starting gate or open ground) using the generated layout data.
3.  **Update Spawn Position:**
    Modify the player placement logic to use this safe location found during generation instead of the fixed coordinates.
4.  **Test Spawn Behavior:**
    Run the game with multiple different random layouts to ensure the player is consistently placed in open space without getting stuck in structures.

## Verification Steps
1.  Launch the game using several different generated layouts.
2.  Observe the view immediately upon spawning.
3.  Confirm the player can move freely and is not stuck inside walls or structures.

## Completion Checks
*   [ ] The player spawns in open, walkable space for any seed.
*   [ ] The spawn is derived from the generated layout rather than hardcoded.
*   [ ] Spawning never places the player inside a solid object.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
