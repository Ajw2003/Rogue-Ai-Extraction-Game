# Plan: Issue 53 - No standalone Unity player build has ever been produced

## Exhaustive Outline
The game currently only runs inside the game engine editor. There is no way to create a standalone version of the game that players can run on their own computers. The goal of this task is to create a tool that builds a standalone version of the game, and then to test that this version works correctly from start to finish.

## Step by Step Execution Instructions

1.  **Create Build Tool:**
    Create a new file in the project to handle the process of building the game.
2.  **Add Build Instructions:**
    Write instructions in this file that tell the game engine how to create a standalone version of the game.
3.  **Add Menu Option:**
    Add a button or menu option to the game engine interface so the build process can be started easily.
4.  **Configure Levels:**
    Make sure the build instructions include all the necessary game levels, starting with the main menu.
5.  **Generate Build:**
    Click the new menu option to create the standalone game.

## Verification Steps
1.  Find the newly created standalone game file on your computer.
2.  Open the game using this file.
3.  Check that the game loads the main menu successfully.
4.  Start a new game session from the main menu.
5.  Complete the game session and check that the game does not crash or freeze.

## Completion Checks
*   [ ] A new tool exists to handle building the game.
*   [ ] A button or menu option exists to start the build process.
*   [ ] A standalone version of the game has been created successfully.
*   [ ] The standalone game opens and reaches the main menu.
*   [ ] A full game session can be started and completed in the standalone game.


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
