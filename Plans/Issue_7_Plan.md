# Plan: Issue 7 - Missing Crosshair UI

## Exhaustive Outline
Currently, there is no crosshair on the screen while playing the game. This makes it very difficult for the player to know where they are aiming when casting spells or trying to pick up loot. The game already knows where the player is looking, but it does not display a visual indicator. The goal of this task is to add a dynamic crosshair to the player's screen. The crosshair must appear during active gameplay, change its appearance when pointing at something the player can interact with, and disappear when a menu is open.

## Step by Step Execution Instructions

1.  **Create Crosshair Graphics:**
    Create or locate simple image files for the crosshair. You will need a default dot or plus sign, and an alternate version (like an open hand or highlighted shape) for when the player is aiming at an interactive object.

2.  **Update the HUD System:**
    Open `RaidHudView.cs`. Add a new UI element to display the crosshair graphic exactly in the center of the screen.

3.  **Implement State Logic:**
    Add code to check the current game state. The crosshair should only be visible when the state is `GameState.Playing`. Hide it when the game is paused, when a menu is open, or when the player is in the Lair.

4.  **Implement Interaction Feedback:**
    Modify the script that handles object interaction, such as `LootInteractor`. When the aiming system detects an interactive object within reach, it should signal the HUD to swap the default crosshair graphic for the interaction graphic. When the object is no longer in reach, revert to the default graphic.

## Verification Steps
1.  Launch the game and enter a playable level.
2.  Look around the environment to verify the default crosshair is in the center of the screen.
3.  Walk up to a piece of loot and aim at it. Verify the crosshair changes appearance.
4.  Open the pause menu or a settings menu. Verify the crosshair disappears.
5.  Close the menu and verify the crosshair returns.

## Completion Checks
*   [ ] A crosshair is clearly visible in the center of the screen during active gameplay.
*   [ ] The crosshair changes its visual state when aiming at an interactable object.
*   [ ] The crosshair is hidden during menus, pauses, and while in the Lair.
*   [ ] Aiming and interacting with objects feels accurate to where the crosshair points.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
