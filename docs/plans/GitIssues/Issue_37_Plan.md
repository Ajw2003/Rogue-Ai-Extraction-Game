# Plan: Issue 37 - No melee combat system exists

## Exhaustive Outline
Right now, weapons in the game can only be thrown at enemies. Even though weapons have stats for how far they reach, how heavy they are, and how loud they are, players cannot swing them in close combat. The goal of this task is to add a dedicated button and rules for swinging weapons. This new setup will use the weapon's reach to see if an enemy gets hit, use its weight to change how fast you swing and how much damage you do, and create sounds that enemies can hear. The Arming Sword will be set up first as a complete working example.

## Step by Step Execution Instructions

1.  **Add a Swing Button:**
    Create a new player control button specifically for swinging a weapon. This must be a separate action from throwing.
2.  **Create the Swing Action:**
    Build the rules that run when the player presses the swing button. This should look at the weapon's weight stat to decide how fast the swing happens.
3.  **Check for Hits:**
    Add a way to check if the swing hits an enemy in front of the player. This must use the weapon's reach stat to determine how far away an enemy can be struck.
4.  **Calculate Damage:**
    Update the rules so that when a hit connects, the damage dealt is based on the weapon's weight stat.
5.  **Connect the Noise:**
    Make the swing action send a sound alert to the game's enemy hearing system. This ensures enemies can hear you swinging the weapon.
6.  **Set Up the Arming Sword:**
    Update the Arming Sword to use all these new features. Make sure it works completely from the moment the button is pressed to dealing damage and making noise.

## Verification Steps
1.  Start the game and pick up the Arming Sword.
2.  Press the new swing button and confirm the character swings the sword instead of throwing it.
3.  Swing at an enemy from different distances to confirm that hits only land when the enemy is within the sword's reach.
4.  Swing the sword near an enemy who is looking away and confirm they turn around or react to the noise.

## Completion Checks
*   [ ] A dedicated button exists for swinging weapons.
*   [ ] Hit distance is controlled by the weapon's reach stat.
*   [ ] Swing speed and damage are affected by the weapon's weight stat.
*   [ ] Swinging a weapon creates a noise alert for the enemy hearing system.
*   [ ] The Arming Sword is completely set up and working as a test example.


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
