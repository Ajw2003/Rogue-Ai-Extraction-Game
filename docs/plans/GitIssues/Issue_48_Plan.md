# Plan: Issue 48 - Casting is invisible to teammates - "friends hear the word half a heartbeat before it resolves" isn't represented

## Exhaustive Outline
Currently, players have no way of knowing when a teammate is in the process of casting a spell until the spell actually occurs. The goal of this task is to add a small update that lets teammates know someone has started casting. This fulfills the game's core concept of sensing a spell right before it happens, all without sending any actual voice recordings between computers.

## Step by Step Execution Instructions

1.  **Add Casting Status:**
    Update the player's shared information to include a new status that shows whether they are currently trying to cast a spell.
2.  **Share Casting Start:**
    Change the input controls so that when a player starts the casting action, this new status becomes active and is shared with other players in the game.
3.  **Share Casting End:**
    Make sure that when the player finishes the casting action or the spell is completed, the status is turned off and shared with other players.
4.  **Add Player Cue:**
    Create a simple visual effect or play a sound on the character of the player who is casting so other players can notice the new active status.

## Verification Steps
1.  Start a multiplayer session with at least two players.
2.  Have one player start the action to cast a spell.
3.  Verify that the second player sees or hears the new indicator on the first player's character before the spell effect actually happens.
4.  Verify that no voice recordings are being sent between the players.

## Completion Checks
*   [ ] Nearby teammates get a visible and/or audible cue that a player is mid-cast before the spell resolves.
*   [ ] No raw audio crosses the network (the existing on-device-only voice decision stays intact).


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
