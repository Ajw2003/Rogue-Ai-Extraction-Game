# Plan: Issue 29 - Market mark-up and "spend the leftover after the debt" are unimplemented rules

## Exhaustive Outline
The goal of this task is to implement two economic rules in the game market. First, players should only be able to spend the gold they have left over after their automatic debt payment is taken. Second, any items bought in the market must cost more than their equivalent value if they were found during a raid.

## Step by Step Execution Instructions

1.  **Debt Payment Automation:**
    Update the market logic to ensure the player's debt is automatically paid before any remaining gold is made available for spending.
2.  **Calculate Spendable Leftovers:**
    Implement a calculation to determine how much gold the player has left to use in the market after the debt payment is subtracted from their total wealth.
3.  **Market Price Markup:**
    Adjust the pricing rules for market items so that their purchase cost is always higher than the base value they would have if found as loot in a raid.

## Verification Steps
1.  Complete a raid and observe that the required debt is paid automatically before accessing the market.
2.  Verify that only the remaining gold is available to spend in the market interface.
3.  Compare the market purchase price of several items against their known raid loot values to confirm the market markup is applied.

## Completion Checks
*   [ ] Debt is paid first, automatically, before anything is spendable at the market.
*   [ ] Market prices for an item that also exists as raid loot are provably higher than that loot's raid worth.


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
