# Plan: Issue 13 - Visual Feedback for Spells

## Exhaustive Outline
Currently, casting a spell only prints a text message in the background. No magic effects, particles, or lights appear on the screen. This makes it impossible for the player to see what spell they cast or if it hit anything. Additionally, if the spell fails or misfires, there is no visual indication. The goal of this task is to add clear, unique visual effects for all eight spells in the game. These effects need to show when the spell is cast, where it hits, and whether it misfired or worked perfectly. The size of the effect should also change based on how loudly the player spoke the spell.

## Step by Step Execution Instructions

1.  **Create Spell Effects:**
    Using Unity's particle system or visual effect graph, create a unique visual effect (VFX) for each of the eight spells (IGNIS, FRANGO, LEVO, AURUM VOCO, TONITRUS, SOMNUS, CADAVER SURGE, PORTA). Create one effect for the player's hand during casting, and one for the location where the spell hits or resolves.

2.  **Create Misfire Effects:**
    Create a separate, fizzling or explosive visual effect that will play specifically when a spell misfires or fails. This needs to look clearly different from a successful cast.

3.  **Link Effects to the Spell System:**
    Open the `SpellCastingSystem` and `MisfireEngine` scripts. Add code to trigger these visual effects whenever a spell is cast. Ensure the correct effect is chosen based on which spell was used. Trigger the misfire effect instead if the `MisfireEngine` determines the spell failed.

4.  **Implement Volume Scaling:**
    Modify the code that spawns the visual effects. Read the volume level of the cast (whisper, normal, or shout). Scale the size, brightness, or particle count of the visual effect up or down based on this volume level.

## Verification Steps
1.  Launch the game and enter a playable level.
2.  Cast each of the eight spells one by one. Verify a unique effect plays at the player's hand and at the target location for each spell.
3.  Intentionally cast a spell poorly to trigger a misfire. Verify the unique fizzle or explosion effect plays instead of the normal spell effect.
4.  Cast a spell quietly (whisper) and then cast the same spell loudly (shout). Verify the louder spell produces a much larger or brighter visual effect than the quiet one.

## Completion Checks
*   [ ] All eight spells have distinct visual effects for both casting and hitting a target.
*   [ ] A failed spell clearly shows a unique misfire visual effect.
*   [ ] The size and intensity of the visual effects correctly change based on the volume of the cast.
*   [ ] The spell effects appear in the correct physical locations (at the player's hand and at the target).


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
