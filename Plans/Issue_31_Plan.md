# Plan: Issue 31 - Build a 3D Lair space to replace the menu screen

## Exhaustive Outline
The game currently uses a flat menu screen for the player's home base. To match the rest of the game's immersive style, this needs to be replaced with a physical 3D room that the player can walk around in. The room should look damp and lived-in. All previous menu options (checking debt, viewing banked gold, choosing a time period, and starting a journey) need to be moved to physical objects within this new 3D space so the player can interact with them directly.

## Step by Step Execution Instructions

1.  **Create the Room Model:**
    Build or assemble a basic 3D room to serve as the lair. Add details to make it look damp and lived-in.
2.  **Set Up the Scene:**
    Create a new level in the game engine for this room and place the player's first-person character inside it.
3.  **Create Interactive Objects:**
    Add objects in the room to represent the old menu options. For example, add a ledger for debt and banked gold, a map for choosing the era, and a door to leave the area.
4.  **Connect the Logic:**
    Link these physical objects to the existing game functions so the player can interact with them to perform the required actions.
5.  **Remove the Old Menu:**
    Delete the old flat screen menu and update the game flow to load the new 3D room instead.

## Verification Steps
1.  Launch the game and confirm the player starts in or can enter the new 3D lair.
2.  Walk around the room to ensure the player moves correctly in first-person.
3.  Interact with the object for debt and banked gold to confirm the correct numbers show up.
4.  Interact with the map or era selector to ensure the era can be chosen.
5.  Interact with the exit door to verify the player can successfully start a journey.

## Completion Checks
*   [ ] A 3D room for the lair is created and looks damp and lived-in.
*   [ ] The player can walk around the lair in first-person.
*   [ ] The old flat lair menu is completely removed.
*   [ ] Debt and banked gold can be checked by interacting with the environment.
*   [ ] Era selection and starting a journey can be done by interacting with the environment.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
