# Plan: Issue 28 - Author wares and pricing for all four market stalls

## Exhaustive Outline
This task involves defining the specific items sold at the four market stalls (Spellmonger, Smith, Alchemist, and Curiosity Dealer) and setting their prices. We will create item files for these wares, establish the rules for how unknown spells are unlocked for purchase, ensure weapon upgrades have a system they can be applied to, and price all market items higher than the versions found in the dungeon to encourage exploration.

## Step by Step Execution Instructions

1.  **Define Spell Unlock Rules:**
    Determine and write the rules for how unknown spells become available at the Spellmonger stall.
2.  **Create Spellmonger Items:**
    Create the item files for the unknown spells and syllable coaching wares, and set their prices.
3.  **Check Melee Weapon System:**
    Verify if the melee weapon system is ready. If not, wait for that task to finish to ensure there is a way to apply Smith stall upgrades.
4.  **Create Smith Items:**
    Create the item files for blade, reach, and heft upgrades, and set their prices.
5.  **Create Alchemist Items:**
    Create the item files for one-use potions and tonics, and set their prices.
6.  **Create Curiosity Items:**
    Create the item files for gadgets, curios, and other miscellaneous upgrades, and set their prices.
7.  **Audit Pricing:**
    Review the prices of all newly created market items to guarantee they cost more than the exact same items found in the dungeon.

## Verification Steps
1.  Launch the game and visit the Spellmonger stall to confirm unknown spells are properly locked and available for purchase at the correct price.
2.  Visit the Smith stall to purchase an upgrade and verify it can be applied to a weapon.
3.  Visit the Alchemist and Curiosity Dealer stalls to confirm their items are listed with the correct details and prices.
4.  Compare the market prices of several items against their dungeon drop values to ensure the market items are always more expensive.

## Completion Checks
*   [ ] Spell unlock rules are defined and working
*   [ ] Item files are created for all wares across the four stalls
*   [ ] Smith stall upgrades can be successfully applied to weapons
*   [ ] All market items are priced strictly higher than their dungeon equivalents


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
