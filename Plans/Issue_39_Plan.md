# Plan: Issue 39 - Ranged weapons have no aim or fire implementation

## Exhaustive Outline
The game currently lacks a way to use ranged weapons. The goal of this task is to create the features needed to aim, shoot, and reload ranged weapons, starting with the Crossbow. We need to capture the specific feeling described in the game design (firing a single powerful shot followed by a slow and vulnerable reloading process). This requires adding player controls for aiming, visual indicators for ammunition and reload progress, and ensuring that firing the weapon creates an appropriate sound to alert nearby enemies.

## Step by Step Execution Instructions

1.  **Weapon Status Tracking:**
    Add information to weapons to track whether they are loaded, empty, or currently being reloaded.
2.  **Aiming Controls:**
    Create a control input that allows the player to enter an aiming stance. Adjust the camera and character movement to focus on aiming while this button is held.
3.  **Firing Action:**
    Create the action that happens when the player shoots. This should consume ammunition, launch the shot, and play a loud sound.
4.  **Reload Sequence:**
    Build the reload process. This must be a slow, deliberate action that leaves the player vulnerable. It should take a set amount of time to complete before another shot can be fired.
5.  **Player Screen Updates:**
    Add visual elements to the screen to show the player their current ammunition count and a clear indicator of how much time is left in the reload process.
6.  **Crossbow Setup:**
    Apply these new features to the Crossbow item. Set its reload time to be significantly long to match the design description.

## Verification Steps
1.  Equip a Crossbow in the game.
2.  Hold the aim button and verify the camera and character respond appropriately.
3.  Press the fire button and confirm a shot is fired, ammunition is consumed, and a loud sound is produced.
4.  Observe the reload process. Ensure it takes a long time and that you can see a visual indicator of the reload progress on the screen.
5.  Try to fire again while reloading and confirm the weapon does not shoot.

## Completion Checks
*   [ ] At least one ranged weapon (the Crossbow) is fully playable with aim and fire controls.
*   [ ] Firing the weapon triggers a long reload period where the player cannot fire again immediately.
*   [ ] Firing the weapon creates a noise that can be heard in the game world.
*   [ ] The current ammunition count or reload progress is clearly visible on the screen.
