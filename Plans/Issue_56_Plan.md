# Plan: Issue 56 - Settings menu completeness (audio mix, keybinds, mic device) is unverified

## Exhaustive Outline
The settings menu needs to be fully built out to support the needs of our voice-controlled cooperative game. Currently, it is unknown if the settings menu has the necessary features. The goal of this task is to ensure the player can adjust their master volume, sound effects, and music levels. Additionally, we need to guarantee that players can change their keyboard controls, specifically the backup buttons used for casting spells. Finally, the player must be able to select their preferred microphone and clearly see which one is currently listening.

## Step by Step Execution Instructions

1.  **Check Existing Settings:**
    Open the current settings menu files to see what is already built.
2.  **Add Volume Controls:**
    Add three sliders to the menu for master volume, music, and sound effects. Connect these sliders to the game audio system.
3.  **Create Key Binding Options:**
    Add a list of all game actions, including the backup buttons for all spell words. Make sure clicking an action allows the player to press a new key to reassign it.
4.  **Build Microphone Selection:**
    Add a dropdown menu that lists all microphones connected to the computer.
5.  **Add Active Microphone Visual:**
    Place a small visual icon next to the microphone dropdown that lights up when the selected microphone hears sound.
6.  **Save Settings:**
    Ensure all chosen settings are saved when the game closes and loaded correctly when the game opens again.

## Verification Steps
1.  Open the game and navigate to the settings menu.
2.  Move the volume sliders and confirm the game audio gets louder or quieter based on the slider positions.
3.  Change the key for a spell word to a new button, start a game session, and press the new button to confirm it casts the spell.
4.  Plug in a second microphone, open the settings, and confirm both microphones appear in the dropdown list.
5.  Select a microphone and speak into it to verify the visual indicator lights up.

## Completion Checks
*   [ ] Master, sound effects, and music volume sliders are present and working.
*   [ ] All gameplay actions and spell word backups can be reassigned to new keys.
*   [ ] The player can choose a specific microphone from a list of connected devices.
*   [ ] A visual indicator correctly shows that the chosen microphone is active and working.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
