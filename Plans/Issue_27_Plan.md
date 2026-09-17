# Plan: Issue 27 - Build the market hub - a place, a shop UI, and persistent "stays bought" state

## Exhaustive Outline
Currently, players can only spend their gold on paying off their debt. There is no place in the game to buy items or upgrades. The goal of this task is to create a market hub that players can access from their home base. This market will have four distinct shops. When a player buys an item, the cost should be deducted from their saved gold. These purchases must be permanently saved to the player's account so that if they leave the game and come back later, the items remain purchased and cannot be bought again.

## Step by Step Execution Instructions

1.  **Create the Market Area:**
    Build a new market scene or a menu that the player can easily enter from their home base.
2.  **Set Up the Four Shops:**
    Add four separate shop sections or stalls within the market.
3.  **Connect to Player Gold:**
    Link the shops to the player's saved gold so that buying an item correctly subtracts the cost from their balance.
4.  **Save Purchase History:**
    Update the game's save system to record which items the player has bought.
5.  **Update Shop Displays:**
    Change how the shops look based on the player's purchase history so that already bought items are clearly marked as owned and cannot be purchased again.

## Verification Steps
1.  Start the game and load into the home base.
2.  Navigate to the newly created market area or menu.
3.  Check that there are four separate shops available.
4.  Verify you have some saved gold, then purchase an item from one of the shops.
5.  Check that your saved gold has decreased by the correct amount.
6.  Leave the game completely and restart it.
7.  Return to the market and verify the item you bought is still marked as owned and cannot be bought a second time.

## Completion Checks
*   [ ] A market location or menu is reachable from the lair.
*   [ ] Four stalls are represented in the market.
*   [ ] A purchase is deducted from banked gold.
*   [ ] Purchases are saved and persist across sessions per player.
*   [ ] Re-entering the market shows previously bought items as owned and prevents buying them again.


## Technical Constraints
When executing this plan, you MUST read and strictly adhere to ALL principles and conventions detailed in `docs/UnityConvention.md`. 
You cannot pick and choose which rules to enforce; every single rule applies.
Specifically, you must follow:
- Core Architectural Principles: KISS, YAGNI, Solve the Root Cause, DRY, and SRP.
- All Naming Conventions (e.g., `m_camelCase` for privates, `s_camelCase` for statics, `PascalCase` for methods/properties).
- All Formatting & Syntax rules (e.g., Allman braces, mandatory braces, 4-space indentation).
- Class & Method Organization (Newspaper metaphor, correct layout order).
- Unity-Specific Implementations (e.g., `[SerializeField]` instead of public, `[Tooltip]` instead of comments).
- UI Toolkit (UXML/USS) Naming (BEM convention, kebab-case).
- Commenting rules (Explain 'Why', not 'What').
