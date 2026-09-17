# Plan: Issue 58 - Steam lobby/invite flow has no art or branding pass

## Exhaustive Outline
The game currently has working features for players to invite friends and join lobbies through Steam. However, the visual appearance of the lobby menus does not match the rest of the game. The goal of this task is to update these screens to use the correct colors, fonts (Eczar and Spectral), and visual themes (like vellum and candlelight) so they look consistent with the main menu and other polished screens.

## Step by Step Execution Instructions

1.  **Review Guidelines:** Read the project documentation to understand the required colors, fonts, and visual themes for the game.
2.  **Locate Lobby Files:** Find the interface files responsible for displaying the lobby and invite screens.
3.  **Update Fonts:** Change all text on the lobby screens to use the Eczar and Spectral fonts.
4.  **Apply Colors:** Update the colors of all buttons, text, and backgrounds to match the established project color palette.
5.  **Add Visual Themes:** Add the vellum and candlelight styling to the backgrounds and borders of the lobby menus.

## Verification Steps
1.  Start the game and open the multiplayer lobby menu.
2.  Check that all text uses the correct Eczar and Spectral fonts.
3.  Verify that all colors match the project guidelines.
4.  Confirm that the overall look feels consistent with the main menu and other finished screens.
5.  Receive a game invite from a friend and ensure the invite pop-up also matches the new visual style.

## Completion Checks
*   [ ] Fonts on the lobby menus use Eczar and Spectral
*   [ ] Colors on the lobby menus match the project palette
*   [ ] The visual styling matches the vellum and candlelight theme
*   [ ] The lobby and invite screens look consistent with the main menu


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
