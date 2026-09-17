# Plan: Issue 51 - No camera shake, hit-stop, or impact juice anywhere

## Exhaustive Outline
The game currently lacks visual feedback like camera shaking or brief game pauses when important actions occur. The goal of this task is to add these feedback effects to casting spells, taking damage, and hitting enemies. This will make these actions feel much more impactful and satisfying for the player.

## Step by Step Execution Instructions

1.  **Build a Camera Shake Feature:**
    Create a reusable method to shake the player's view. This feature needs to support different strengths and lengths of time.
2.  **Build a Game Pause Feature:**
    Create a reusable method to briefly freeze the game action upon impact.
3.  **Connect Feedback to Spell Casting:**
    Update the spell casting logic to trigger a camera shake. Ensure the shake strength increases for more powerful spells.
4.  **Connect Feedback to Player Damage:**
    Update the damage taking logic to trigger both a camera shake and a brief game pause.
5.  **Connect Feedback to Melee Attacks:**
    Update the melee attack logic to trigger a camera shake and a brief game pause when an attack lands successfully.

## Verification Steps
1.  Play the game and cast a spell to confirm the camera shakes.
2.  Cast a powerful spell and a weak spell to confirm the shake strength is different.
3.  Let an enemy damage the player character to confirm the camera shakes and the game freezes for a moment.
4.  Hit an enemy with a melee attack to confirm the camera shakes and the game freezes for a moment.

## Completion Checks
*   [ ] Casting a spell causes a camera shake or brief game pause.
*   [ ] Taking damage causes a camera shake or brief game pause.
*   [ ] A melee hit causes a camera shake or brief game pause.
*   [ ] The intensity of the feedback changes based on the strength of the action.


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
