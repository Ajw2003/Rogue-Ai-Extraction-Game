# Plan: Issue 43 - GildedColossus (vault boss) has no unique behaviour, telegraph, or arena design

## Exhaustive Outline
The final boss of the Crypt area, the Gilded Colossus, currently acts like a regular guard and lacks the special feel expected of a rare, high-value fight. The goal of this task is to give the boss a distinct combat style, a clear warning signal before its most dangerous move, and to make sure the room it appears in is large enough to fit its massive size.

## Step by Step Execution Instructions

1.  **Create Custom Boss Actions:**
    Write new instructions for the Gilded Colossus so it stops using the standard guard actions and uses its own unique attack pattern.
2.  **Add Attack Warning:**
    Create a noticeable visual or audio warning that triggers right before the boss performs its heavy attack so the player has time to dodge.
3.  **Expand Room Size:**
    Increase the height and width of the final Crypt room so the massive boss can fit comfortably without touching the walls or ceiling.

## Verification Steps
1.  Launch the game and travel to the final room of the Crypt.
2.  Look around the room to make sure the boss fits perfectly and is not getting stuck in the walls or ceiling.
3.  Start the fight and observe the boss to verify it uses its new unique attack instead of normal guard moves.
4.  Watch closely to confirm that a clear warning happens right before the dangerous attack hits.

## Completion Checks
*   [ ] The boss uses at least one unique attack different from the normal guards.
*   [ ] A clear warning signal appears before the most dangerous attack.
*   [ ] The room is large enough to fit the boss's tall and wide body.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
