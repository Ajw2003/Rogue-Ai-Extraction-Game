# Plan: Issue 30 - EPIC: The Lair is a menu screen, not the physical place the pitch describes

## Exhaustive Outline
The game currently uses a basic menu screen to represent the player's home area. The original design calls for a physical room with a dark and moody atmosphere. This task focuses on replacing the menu screen with an actual 3D room that players can stand in. This space will need to show the player's gold and debt as physical objects rather than numbers on a screen, and it will eventually serve as the location for the market.

## Step by Step Execution Instructions

1.  **Create the Lair Room:**
    Set up a new space in the game to serve as the physical Lair.
2.  **Build the Environment:**
    Place 3D models to create the room, ensuring it feels like a damp and permanent safe space.
3.  **Apply Lighting and Atmosphere:**
    Set up the lighting to be very dark. Use a single candle and a fire as the main light sources to match the intended mood.
4.  **Add Physical Items for Numbers:**
    Replace the flat text for gold and debt with physical objects in the room, such as piles of coins or a written ledger.
5.  **Update Game Flow:**
    Change the game so that entering the Lair places the player into this new 3D room instead of opening a menu.

## Verification Steps
1.  Start the game and travel to the Lair.
2.  Confirm that you load into a 3D environment instead of a menu screen.
3.  Look around the room to ensure the lighting is dark and moody.
4.  Check that your saved gold and debt are visible as physical items in the room.

## Completion Checks
*   [ ] A 3D room is created for the Lair.
*   [ ] The lighting and mood match the dark and safe feeling described in the original design.
*   [ ] Gold and debt are represented physically in the space.
*   [ ] Entering the Lair places the player in the physical room.


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
