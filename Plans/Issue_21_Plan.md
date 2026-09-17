# Plan: Issue 21 - Portal Visuals and Interaction

## Exhaustive Outline
Currently, players escape a raid and return to the lair by walking into an invisible box with a flat green square on the ground. This breaks the fantasy of a magical heist. We need a physical portal object in the game world that looks like a magical gateway. This portal must act as the extraction point. It should also have distinct visual states so players can clearly see when it is open and ready to use versus when it is closed or inactive.

## Step by Step Execution Instructions

1.  **Create the Portal Asset:**
    Acquire or create a 3D model for the portal (like a stone archway). Add a magical visual effect inside the arch to represent the gateway. Ensure you can toggle this effect on and off.

2.  **Replace the Invisible Trigger:**
    Open the `RaidSceneBuilder` script. Find the code that creates the invisible `ExtractionZone` trigger box and the flat green square. Replace this with code that spawns your new 3D portal prefab at the exact same location.

3.  **Implement Visual States:**
    Create a script for the portal prefab. When the extraction zone is unavailable (inactive), the portal's magical effect should be turned off or look dim. When the raid reaches the phase where players can escape, the portal effect should ignite and look active.

4.  **Connect to the Extraction Logic:**
    Ensure the new portal prefab still contains the `BoxCollider` trigger required for extraction. Verify that when a player walks into the active portal effect, it triggers `RaidDirector.RaidResolved` and returns them to the Lair, exactly as the invisible box did.

## Verification Steps
1.  Launch the game and start a raid.
2.  Navigate to the edge of the castle where the extraction zone is located.
3.  Verify you see a 3D portal structure instead of a green square on the ground.
4.  Verify the portal's magical effect is visible if extraction is available, or off if it is not.
5.  Walk into the active portal.
6.  Verify the game successfully extracts you and returns you to the Lair screen.

## Completion Checks
*   [ ] A 3D portal model is present at the extraction point.
*   [ ] The flat green placeholder square is completely removed.
*   [ ] The portal clearly shows whether extraction is currently active or inactive.
*   [ ] Walking into the portal successfully ends the raid and returns the player to the lair.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
