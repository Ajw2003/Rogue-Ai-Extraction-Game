# Plan: Issue 5 - Room Connection and Layout Fix

## Exhaustive Outline
The current procedural generation system places castle rooms on a grid, but the physical rooms do not actually connect. This leaves empty spaces between rooms. Furthermore, the doorways do not align. The goal of this task is to ensure that when the game builds a castle, every room connects seamlessly to its neighbor without gaps, and doorways perfectly align. This will allow the player to walk continuously from the starting point (the crypt) to the exit.

## Step by Step Execution Instructions

1.  **Adjust Room and Grid Spacing:**
    Update the room size or the grid cell size so they match exactly. Currently, rooms are smaller than the grid cells they occupy. Set the grid cell size in `ProceduralCastleGenerator.cs` to match the exact physical size of the room models.

2.  **Align Room Doorways:**
    Modify the generation logic so it checks for doorways (referred to as sockets) before placing a room. When a new room is placed next to an existing room, the system must rotate or select a room so that their doorways connect perfectly.

3.  **Ensure Continuous Pathing:**
    Update the generation loop. If the system tries to place a room and it cannot find a valid connection to an existing doorway, it should try another room type or rotate the room. Do not place rooms that block the main path or have no connected doors.

4.  **Update Validation Tests:**
    Review the automated tests (`CastleGeneratorTests`) that check if a path exists from start to finish. Ensure these tests now check that connected rooms actually share aligned doorways, not just sit next to each other on the grid.

## Verification Steps
1.  Open the Unity editor and run the castle generation script.
2.  Observe the generated layout in the scene view.
3.  Visually confirm there are no physical gaps between adjacent rooms.
4.  Visually confirm that where two rooms meet, a doorway connects them.

## Completion Checks
*   [ ] Adjacent rooms share a physical wall or doorway with zero empty gaps between them.
*   [ ] Doorways always connect to other doorways. A doorway never opens into a solid wall or empty space.
*   [ ] A player character can walk from the starting room to the exit room entirely through interior space.
*   [ ] The automated tests for castle generation determinism and reachability still pass successfully.


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
