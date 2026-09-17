# Plan: Issue 19 - Procedural Generation Grid Spacing

## Exhaustive Outline
Currently, the pieces that make up the castle do not fit together cleanly. The generator assumes every piece is exactly 12 meters wide, but many pieces are slightly larger or smaller. For example, a drawbridge is much longer, and regular rooms are slightly smaller. Because of this, gaps appear between rooms, and some structures do not align properly. The goal of this task is to enforce a strict grid system. We will standardize the size of all castle modules so they fit perfectly into the 12-meter grid, ensuring they touch perfectly without gaps or overlapping.

## Step by Step Execution Instructions

1.  **Standardize Room Footprints:**
    Review all the 3D models for the castle modules. Adjust the models or their bounding boxes in Unity so that their physical footprint exactly matches the `cellSize` (12 meters).
    For non-standard pieces like the `Drawbridge` (currently 19.8m) or `WallCorner` (12.4m), decide if they should span multiple grid cells or be resized to fit exactly one cell. Update the generation script to account for multi-cell pieces if necessary.

2.  **Eliminate the Built-in Gap:**
    The regular rooms are currently 11.2 meters wide inside a 12-meter cell. Either scale the rooms up to 12 meters or reduce the `cellSize` in the `ProceduralCastleGenerator` script to exactly 11.2 meters. This ensures adjacent grid cells physically touch.

3.  **Address Height Differences:**
    Create a rule for how rooms of different heights connect. Ensure that the floor of every single module sits perfectly flush with the ground level (Y coordinate = 0). If a room is taller or shorter, the difference should only affect the ceiling, never the floor.

4.  **Run Automated Tests:**
    After adjusting sizes, run the existing `CastleGeneratorTests`. These tests ensure the castle still builds predictably every time.

## Verification Steps
1.  Open the Unity editor and run the procedural castle generator.
2.  Switch to the scene view and look closely at where two standard rooms meet. Verify there is no gap.
3.  Find a non-standard piece like a drawbridge or wall corner. Verify it fits cleanly into the layout without overlapping or leaving spaces.
4.  Look at the layout from a side angle. Verify the floor of every room sits perfectly flat on the ground.
5.  Check the test runner in Unity and verify the determinism tests pass.

## Completion Checks
*   [ ] The grid cell size matches the actual physical size of the room models perfectly.
*   [ ] Exceptionally large or small pieces are resized or properly configured to occupy exact grid cell multiples.
*   [ ] Neighboring modules touch physically, with zero gap between them.
*   [ ] All room floors rest flat at ground level.
*   [ ] The automated tests for castle generation still pass successfully.


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
