# Plan: Issue 11 - Player Hand and Door Animations

## Exhaustive Outline
Currently, the player character is just an invisible shape with no hands or arms shown on screen. When casting a spell, there is no physical gesture. Additionally, doors in the game instantly snap open or closed instead of swinging. The goal of this task is to add visible hands to the player's view that animate when casting spells. We must also add smooth opening and closing animations to all doors, whether they are opened by hand or pushed open by a spell.

## Step by Step Execution Instructions

1.  **Add First-Person Hands:**
    Obtain a 3D model of hands or arms. Attach this model to the player's camera so it stays in front of their view.
    Create an `AnimatorController` for the hands with an idle state and a casting gesture state.

2.  **Wire Hand Animations to Casting:**
    Open `PushToCastController.cs`. Find the variables for the hand animator and the casting boolean. Assign the newly created hand animator to this script. Ensure the script sets the casting boolean to true when the player begins casting, and false when they finish or stop.

3.  **Add Door Animations:**
    Create a simple animation or use code to smoothly rotate the door models open and closed over a short time (like half a second). Add an `Animator` or a custom rotation script to the door prefab.

4.  **Connect Doors to Interactions and Spells:**
    Open the script that controls doors. Ensure that when a player interacts with a door to open it, the script triggers the smooth opening animation instead of instantly changing its position. Ensure that when the opening spell is cast on a door, it triggers this exact same animation.

## Verification Steps
1.  Launch the game and enter a playable level.
2.  Look down to verify you can see the player's hands or arms.
3.  Cast a spell and verify the hands perform a casting gesture.
4.  Walk up to a closed door and interact with it. Verify the door swings open smoothly over time.
5.  Find another closed door, step back, and cast the door-opening spell. Verify the door swings open using the same smooth animation.

## Completion Checks
*   [ ] The player can see a first-person hand or arm model on screen.
*   [ ] The hands play an animation while the player is casting a spell.
*   [ ] Doors open and close with a smooth visual motion, not an instant snap.
*   [ ] Doors use the same smooth motion regardless of whether they are opened by hand or by magic.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
