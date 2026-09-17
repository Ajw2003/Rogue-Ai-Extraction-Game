# Plan: Issue 38 - Weapon roster doesn't match the pitch's twelve named weapons

## Exhaustive Outline
The game currently has eight incorrectly named weapon models, but the original design pitch requires twelve specific weapons with distinct descriptions and statistics. This task aims to fix the weapon list to match the original pitch. It includes renaming or remaking the existing items, fixing a historical mismatch between a flintlock and a wheellock pistol, and ensuring every weapon has the correct Reach, Heft, and Noise statistics. If we decide not to use the original list, we must officially document that choice.

## Step by Step Execution Instructions

1.  **Compare Lists:**
    Review the current eight models in the weapons folder and compare them to the twelve weapons required by the pitch document.
2.  **Resolve the Pistol Mechanism:**
    Decide whether to keep the current flintlock pistol or change it to the historically accurate wheellock pistol.
3.  **Document Changes:**
    If choosing to keep the flintlock or change any other weapons from the original pitch, write down the reasons for this decision in the project decisions document.
4.  **Update Weapon Models:**
    Rename existing models to match the twelve pitched weapons. Create placeholder models for the missing items (like the Khopesh, Sling, and Caltrops).
5.  **Assign Statistics:**
    Update each of the twelve weapons with the correct Reach, Heft, and Noise numbers as defined in the pitch document.

## Verification Steps
1.  Open the game editor and check the weapons folder to confirm all twelve weapons are present and correctly named.
2.  Select each weapon in the editor and look at its properties to verify that the Reach, Heft, and Noise values are entered correctly.
3.  If changes were made to the original plan, open the decisions document and confirm the reasons are clearly written.

## Completion Checks
*   [ ] The final weapon list matches the twelve names from the pitch or has documented reasons for any differences.
*   [ ] The pistol mechanism issue (wheellock versus flintlock) is resolved and corrected.
*   [ ] Every weapon has the correct Reach, Heft, and Noise values applied.
*   [ ] The decisions document is updated if the final roster differs from the original twelve weapons.


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
