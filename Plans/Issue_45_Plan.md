# Plan: Issue 45 - Drawbridge has no operable mechanism despite existing as a modelled room

## Exhaustive Outline
The drawbridge currently exists as a static 3D model in the castle environment but lacks any functionality. The goal of this task is to make the drawbridge operable by giving it a raised and lowered state. This state will be controlled by the game's existing alarm system, causing the drawbridge to raise (close) when the castle goes on high alert, similar to how doors automatically lock during a lockdown. This action needs to be accompanied by visible animations and sound effects so players are aware of the change.

## Step by Step Execution Instructions

1.  **Create Drawbridge Script:**
    Write a new script to manage the drawbridge's state (raised or lowered).
2.  **Integrate with Alarm System:**
    Connect the drawbridge script to the existing alarm system so that it listens for changes in the alarm state (such as when the household is roused).
3.  **Implement State Logic:**
    Program the drawbridge to automatically raise when the alarm reaches the required threshold, mimicking the lockdown behavior.
4.  **Add Animation Support:**
    Update the drawbridge prefab to include animations for raising and lowering. Trigger these animations from the drawbridge script when the state changes.
5.  **Add Audio Support:**
    Attach audio components to the drawbridge prefab and trigger sound effects in the script whenever the drawbridge raises or lowers.

## Verification Steps
1.  Start a level containing a castle with a drawbridge.
2.  Observe that the drawbridge is initially in its default lowered position.
3.  Trigger the castle alarm to reach the Roused state.
4.  Verify that the drawbridge visually animates to the raised position.
5.  Listen to confirm that the appropriate raising sound effect plays during the animation.
6.  Attempt to cross the drawbridge to ensure it correctly blocks movement when raised.

## Completion Checks
*   [ ] The Drawbridge module has a raise/lower state.
*   [ ] The state is driven by the alarm system (e.g., barring it at Roused).
*   [ ] The state change is visibly animated.
*   [ ] The state change produces sound effects.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
