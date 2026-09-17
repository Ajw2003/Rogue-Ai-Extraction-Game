# Plan: Issue 52 - Throwing and swinging have no wind-up or follow-through animation weight

## Exhaustive Outline
Currently, when a player throws or swings an object, the action happens instantly without any realistic preparation or recovery movement. This makes the actions feel weightless and unnatural. The goal of this task is to add a wind-up (a brief preparatory motion) and a follow-through (the continued natural movement after the action) to both throwing and swinging. This will give the interactions a much needed sense of weight and impact.

## Step by Step Execution Instructions

1.  **Define Action Phases:**
    Set specific time durations for the wind-up, release, and follow-through parts of the throwing and swinging motions.
2.  **Add Throwing Wind-Up:**
    Update the throwing action so that the object does not launch the moment the button is pressed. Instead, add a brief delay where the object is visually pulled back.
3.  **Apply Throwing Follow-Through:**
    Ensure there is a short recovery period or motion immediately after the object is released to show momentum.
4.  **Sync Object Launch:**
    Adjust the object's movement so it is thrown exactly at the end of the wind-up phase, rather than the start.
5.  **Prepare Melee Swinging:**
    Apply these same three phases (wind-up, impact, and follow-through) to the melee swinging action.

## Verification Steps
1.  Pick up an object in the game and throw it.
2.  Watch closely to ensure the object is pulled back briefly before it flies forward.
3.  Verify that the motion does not feel instant and that there is a smooth visual completion after the throw.
4.  Equip a melee weapon (if the system is ready) and swing it to check for the same preparation and recovery phases.

## Completion Checks
*   [ ] A throw action includes a noticeable wind-up phase before the object is released.
*   [ ] A throw action includes a follow-through phase after the object is released.
*   [ ] The throwing velocity is applied at the correct moment, not instantly.
*   [ ] The same wind-up and follow-through logic is prepared or applied for melee swings.


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
