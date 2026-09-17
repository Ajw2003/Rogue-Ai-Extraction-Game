# Plan: Issue 22 - Comprehensive Audio and Visual Effects

## Exhaustive Outline
Currently, the game is completely silent and visually flat. Spells, enemy attacks, menu clicks, and even the noise simulation exist only as numbers in the code. Because there are no sounds or particle effects, the player cannot tell what is happening. The goal of this task is to bring the game to life by adding sound effects (SFX) and visual effects (VFX) to all major actions. This includes sounds and particles for casting spells, getting hit, breaking loot, and clicking buttons in the menu. All sounds must also be routed through a proper audio mixer so their volumes can be balanced.

## Step by Step Execution Instructions

1.  **Implement Audio Mixer:**
    Create an `AudioMixer` asset in Unity. Set up distinct audio groups for Master, Music, Sound Effects (SFX), and User Interface (UI). Ensure every `AudioSource` you create later routes into one of these specific groups, never bypassing the mixer.

2.  **Add Spell and Combat Effects:**
    Building on previous VFX tasks, attach `AudioSource` components to the player and enemies. When a spell is cast, plays a whoosh or magical sound. When a spell misfires (fails), play a distinct fizzle sound and visual effect. When an enemy attacks or hits the player, play a swing or impact sound, alongside a blood or spark visual effect.

3.  **Add Interaction and Loot Effects:**
    Find the `LootPickup._brokenVfx` hook and assign a shattering visual effect to it. Add a glass-breaking or wood-splintering sound when loot is destroyed. Add a subtle sound when picking up an item successfully.

4.  **Add UI and System Sounds:**
    Open the menu scripts. Add a click or chime sound that plays whenever the player presses a button, transitions between menus, or when the extraction portal activates.

## Verification Steps
1.  Launch the game and navigate the main menu. Verify you hear sounds when clicking buttons.
2.  Start a raid. Cast a spell successfully and verify you see a visual effect and hear a magical sound.
3.  Intentionally misfire a spell. Verify you hear a distinct failure sound and see a fizzle effect.
4.  Engage an enemy. Verify you hear their weapon swing and hear an impact sound if they hit you.
5.  Find a piece of loot and intentionally break it or drop it from a height. Verify you see it shatter and hear a breaking sound.

## Completion Checks
*   [ ] Every major action (spells, attacks, breaking loot) has both a visual particle effect and a sound effect.
*   [ ] Misfiring a spell produces a clear, unique sound and visual failure.
*   [ ] UI elements produce sound effects when clicked or activated.
*   [ ] All audio in the game is routed through an `AudioMixer` instead of playing raw.


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
