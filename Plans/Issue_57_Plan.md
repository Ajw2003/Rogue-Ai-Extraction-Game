# Plan: Issue 57 - "Every word can also be bound to a key" has no real settings-exposed keyboard-casting mode

## Exhaustive Outline
The game needs a way for players to cast spells using their keyboard instead of their voice. Right now, the game only uses keyboard casting as a backup if it cannot find a microphone. We need to add a setting that lets players choose to use the keyboard on purpose, even if their microphone works perfectly. This ensures that players who cannot speak, prefer not to speak, or are playing late at night can still enjoy the game. Additionally, we need to make sure every magical word in the game has a specific key assigned to it, and players must be able to change these keys to fit their preferences.

## Step by Step Execution Instructions

1.  **Create the Settings Toggle:**
    Add a new switch in the options menu for keyboard casting. Label it clearly so players know it replaces voice controls with keyboard buttons.
2.  **Update the Input Selection:**
    Change the game logic so it checks the new settings switch first. If the setting is on, the game must use the keyboard input system instead of looking for a microphone.
3.  **Define Default Keys:**
    Set up default keyboard keys for all 40 words in the game dictionary. Ensure these keys do not overlap with basic game controls like walking or jumping.
4.  **Enable Custom Key Bindings:**
    Add a section in the controls menu where players can see the 40 magical words and change the buttons assigned to them.
5.  **Connect Keys to Spells:**
    Update the keyboard input system to read the new key bindings and send the correct words to the game when the buttons are pressed.

## Verification Steps
1.  Open the game and plug in a working microphone.
2.  Go to the options menu and turn on the keyboard casting setting.
3.  Try to cast a spell using the microphone, and verify that the game ignores the voice input.
4.  Press the default keys for various magical words and verify that the game registers the words correctly.
5.  Go to the controls menu and change the key for a specific word to a new button.
6.  Press the new button and verify that the game registers the correct word.

## Completion Checks
*   [ ] A settings toggle lets a player choose keyboard casting even when a working microphone is available.
*   [ ] Each of the 40 magical words has a clear, documented default key.
*   [ ] The player can rebind the key for every single magical word.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
