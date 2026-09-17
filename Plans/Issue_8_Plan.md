# Plan: Issue 8 - Mouse Cursor Lock and Visibility

## Exhaustive Outline
During gameplay, the mouse cursor remains visible and can move off the game screen. This disrupts the camera movement and breaks immersion, as looking around with the mouse fights against the computer's normal cursor behavior. The goal of this task is to ensure the cursor is properly hidden and locked to the center of the screen when the player is actively moving and exploring. It must also correctly reappear and unlock whenever the player opens a menu or leaves the active gameplay state.

## Step by Step Execution Instructions

1.  **Locate Cursor Management:**
    Identify a central script to handle cursor state, or use the existing game state manager. It is generally better to manage the cursor based on `GameState` rather than individual camera scripts like `FreeLookPlaytestController`.

2.  **Lock and Hide During Play:**
    Add logic so that when the game state changes to `GameState.Playing`, the system sets `Cursor.lockState` to `CursorLockMode.Locked` and `Cursor.visible` to `false`.

3.  **Unlock and Show for Menus:**
    Add logic so that when the game state changes to `MainMenu`, `Lair`, `Paused`, `Inventory`, or `Settings`, the system sets `Cursor.lockState` to `CursorLockMode.None` and `Cursor.visible` to `true`.

4.  **Handle Window Focus:**
    Implement Unity's `OnApplicationFocus` method in the cursor management script. If the game loses focus (for example, if the player alt-tabs to another program), the cursor should unlock and become visible. When the game regains focus, the cursor should re-lock and hide only if the current game state is still `GameState.Playing`.

## Verification Steps
1.  Launch the game and enter a playable level.
2.  Move the mouse around to verify the camera moves smoothly and no cursor is visible on the screen.
3.  Press the escape key or open a menu. Verify the cursor immediately appears and can click on buttons.
4.  Close the menu and verify the cursor disappears again.
5.  Press alt-tab to switch to another application, then switch back to the game. Verify the cursor state recovers correctly based on whether you are in a menu or in gameplay.

## Completion Checks
*   [ ] The mouse cursor is locked and completely hidden during active gameplay.
*   [ ] The mouse cursor is unlocked and visible in all menus.
*   [ ] Alt-tabbing or losing window focus correctly frees the cursor.
*   [ ] Returning to the game restores the correct cursor behavior without getting stuck hidden or visible.
