# Plan: Issue 33 - Debt and hoard have no in-fiction representation - only HUD numbers

## Exhaustive Outline
The player's debt and collected treasure currently only show up as numbers on the screen. This makes the main goals of the game feel less important. The objective of this task is to add physical items to the player's 3D lair that show exactly how much debt they owe and how much treasure they have safely brought home. For example, a ledger book could track what is owed, and physical piles of gold could grow as the player hoards more treasure.

## Step by Step Execution Instructions

1.  **Design the physical trackers:** Decide what specific objects will represent the debt (like a ledger book or a bill) and the treasure (like piles of coins, gems, or chests).
2.  **Gather the 3D models:** Create or find the necessary 3D shapes for the debt object and the various sizes of treasure piles.
3.  **Place the debt object:** Put the chosen debt tracker in a highly visible spot inside the lair.
4.  **Update the debt object:** Make sure the debt object visually changes or clearly displays the correct amount owed when the player interacts with it or looks at it.
5.  **Place the treasure:** Set up specific areas in the lair where the collected treasure will be displayed.
6.  **Grow the hoard:** Program the treasure areas to show more items or larger piles as the player brings more loot back to the lair.

## Verification Steps
1.  Start the game and travel to the player's lair.
2.  Look for the physical debt object and confirm it clearly shows the starting amount owed.
3.  Complete a task to collect some treasure and return to the lair.
4.  Verify that the newly collected treasure physically appears in the designated areas of the lair.
5.  Check that the debt object updates correctly if a payment is made.

## Completion Checks
*   [ ] The debt is represented as a physical object in the lair space.
*   [ ] Collected loot is visually present in the lair space.
*   [ ] Both physical representations accurately reflect the player's current totals.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
