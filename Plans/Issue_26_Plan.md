# Plan: Issue 26 - EPIC: The Mystical Market pillar does not exist

## Exhaustive Outline
The game's design document outlines a Mystical Market where players can buy items, but this feature is completely missing from the game right now. The goal is to build this market from scratch. It will include a central hub with different shops like the spellmonger, smith, alchemist, and curiosity dealer. The market will have special rules: items here cost more than they do in dungeons, shops remember what a player has bought, and players must use their remaining money here after paying their debts.

## Step by Step Execution Instructions

1.  **Create Market Hub and Interface:**
    Build the main market area that players can walk to from their lair. Add menus and screens for the different shop stalls so players can interact with them.
2.  **Add Shop Items and Prices:**
    Set up the different stalls (spellmonger, smith, alchemist, curiosity dealer) with the items they sell and assign their specific prices.
3.  **Implement Market Rules:**
    Program the special market rules so that items cost more compared to dungeon prices. Ensure that shops remember past purchases and that players are forced to spend their leftover money here after their debt is paid.

## Verification Steps
1.  Start the game and walk from the lair into the new market area.
2.  Open the shop menus to check if items are listed with their higher prices.
3.  Buy an item while owing debt, then return later to confirm the shop remembers the purchase.
4.  Check that any money left over after paying debt is forced to be spent in the market.

## Completion Checks
*   [ ] Market hub area and shop menus are built.
*   [ ] Stalls have their specific items and higher prices assigned.
*   [ ] Shops remember what the player has previously purchased.
*   [ ] Leftover money is correctly forced to be spent in the market.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
