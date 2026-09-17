<Plan: Issue 24 - EPIC: build the game loop end to end so there is something to actually play>
## Exhaustive Outline
The core systems for the game exist but they are not connected into a playable game loop. The goal of this task is to tie these separate systems together so a player can complete the full game loop. The target loop involves starting at the lair, entering the castle, finding loot, dealing with enemies, extracting through a portal, returning to the lair, paying off debt, and repeating the process.

## Step by Step Execution Instructions

1.  **Castle Navigation:**
    Fix issues that prevent the player from walking through the castle. Connect rooms properly, ensure modules are spaced exactly without floating, adjust the player size so they fit in the rooms, and fix the player spawn so they do not spawn inside geometry.
2.  **Loot Mechanics:**
    Ensure loot is discoverable and carryable. Stop loot from being flung across the map when it spawns and add carryable objects to the game world.
3.  **Combat Resolution:**
    Implement a health and damage model so combat can be won or lost. Add feedback for taking and dealing damage, and provide a way to test combat.
4.  **Extraction Implementation:**
    Add a portal asset and place the portal in the game world so extraction exists in the fiction.
5.  **Readable Feedback:**
    Add a crosshair, lock the mouse, and stop the player from moving while in the menu. Add animations for enemies, the player, and doors. Add visual effects for spells, and add visual and sound effects everywhere so the game state is readable.

## Verification Steps
1.  Launch the game and complete the full game loop twice in a row without any outside explanation.
2.  Ensure that each stage of the loop has both a failure state and a success state.
3.  Confirm that the reason to play again is clear in the game's story, such as paying off the debt, rather than just seeing numbers change.

## Completion Checks
*   [ ] A player can walk through the castle without getting stuck.
*   [ ] Loot can be found and carried by the player.
*   [ ] Combat can be won or lost with clear health and damage models.
*   [ ] The player can extract through a portal.
*   [ ] The game provides readable feedback through animations, visual effects, and sound effects.
*   [ ] A player can complete the full loop twice in a row without an operator explaining anything.
*   [ ] Each stage has a fail state as well as a success state.
*   [ ] The reason to go again is legible in the fiction (the debt).
</Plan: Issue 24 - EPIC: build the game loop end to end so there is something to actually play>


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
