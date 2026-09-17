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
