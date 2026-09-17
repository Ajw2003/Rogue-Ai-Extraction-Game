# Plan: Issue 44 - Door/socket types (murder-hole, arrow-loop) are layout tags only, not functional hazards

## Exhaustive Outline
Currently, structural features like murder-holes and arrow-loops are only used to connect rooms together when building the castle. They do not have any actual gameplay purpose. The goal of this task is to make at least one of these features (specifically the murder-hole) function as a real trap or hazard in the game. This will make exploring the castle feel more dangerous and strategic. We also need to update the castle documentation to explain how these features work during gameplay.

## Step by Step Execution Instructions

1.  **Update Murder-Hole Logic:**
    Modify the game code so that rooms with a murder-hole socket type spawn a hazard or trap in that specific location.
2.  **Add Gameplay Effects:**
    Ensure the new murder-hole hazard can interact with characters, such as dropping items or firing projectiles at anyone passing below it.
3.  **Update Castle Documentation:**
    Open the file `docs/systems/castle.md`. Add a new section explaining the gameplay effects of the murder-hole alongside its existing description as a connection point.

## Verification Steps
1.  Launch the game and enter a castle layout that contains a murder-hole socket.
2.  Walk a character underneath or near the murder-hole and verify that the hazard triggers correctly.
3.  Read the `docs/systems/castle.md` file to confirm the new gameplay rules are clearly explained.

## Completion Checks
*   [ ] At least one socket type (such as the murder-hole) has a functional gameplay effect as a hazard.
*   [ ] The new hazard effect is documented in `docs/systems/castle.md`.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
