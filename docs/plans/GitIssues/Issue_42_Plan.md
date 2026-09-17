# Plan: Issue 42 - Guards are silent - no patrol murmur, alert bark, or chase shout

## Exhaustive Outline
Currently, the game calculates how sound travels so guards can react to noises, but the guards do not make any audible sounds themselves. The goal of this task is to give the guards voices that match their current awareness level (calm, suspicious, or chasing the player). These voices need to be played through the game's standard audio system so they sound natural and follow the same rules as other noises in the environment.

## Step by Step Execution Instructions

1.  **Locate Guard State Logic:** Find the code that manages the guard's current behavior and tracks their level of alertness.
2.  **Define Voice Triggers:** Update the guard behavior so that entering a new awareness state triggers a specific voice response. Assign a murmuring sound for when they are calm, a short bark for when they are suspicious, and a shout for when they are actively chasing.
3.  **Connect to Audio System:** Link these new voice triggers to the game's existing sound broadcasting system so the player can hear the voices correctly positioned in the game world.

## Verification Steps
1.  Start the game and approach a guard without being seen or heard.
2.  Listen to verify the guard is making calm murmuring sounds while on patrol.
3.  Make a small noise to make the guard suspicious and verify they play an alert sound.
4.  Let the guard see you and verify they shout loudly to start a chase.

## Completion Checks
*   [ ] Guards make a distinct sound for each level of awareness.
*   [ ] All guard sounds play through the standard sound broadcasting system.


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
