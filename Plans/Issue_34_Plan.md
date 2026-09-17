# Plan: Issue 34 - EPIC: No icon set exists anywhere - spells, weapons, loot, eras, status effects

## Exhaustive Outline
The game currently lacks visual icons for essential features like spells, weapons, loot, time periods, and character conditions (such as burning or sleeping). This prevents the player menus from being fully built. The goal of this task is to design and add a complete set of icons for all these categories into the project. These icons must strictly follow the game's established color rules, specifically using a green-blue color for magical items and reserving yellow-orange exclusively for gold or valuable items.

## Step by Step Execution Instructions

1.  **Folder Creation:**
    Create an "Icons" folder inside the main project art folder. Inside this folder, create subfolders for spells, weapons, loot, time periods, and conditions.
2.  **Color Rule Review:**
    Read the game color guide to memorize the specific rules for using green-blue and yellow-orange colors before drawing.
3.  **Spell Icon Art:**
    Draw and save at least eight distinct icons representing the game's spells.
4.  **Weapon Icon Art:**
    Draw and save one icon for every weapon type available in the final game.
5.  **Loot Icon Art:**
    Draw and save one icon for each loot category. Ensure the yellow-orange color is strictly used only for gold or highly valuable items.
6.  **Time Period Icon Art:**
    Draw and save four separate icons representing the four distinct time periods.
7.  **Condition Icon Art:**
    Draw and save condition icons for burning, stunned, and sleeping states.
8.  **Project Import:**
    Place all completed images into their correct folders within the game project.

## Verification Steps
1.  Open the project files and confirm the new icon folders exist in the correct art directory.
2.  Count the imported images in each folder to ensure all minimum requirements are met (8 spells, 4 time periods, and all weapons, loot, and condition icons).
3.  Visually inspect the icons to ensure they strictly follow the color rules for magic and gold.

## Completion Checks
*   [ ] Art folders for icons are created and organized.
*   [ ] At least 8 spell icons are completed and imported into the project.
*   [ ] Weapon icons (one for each weapon) are completed and imported into the project.
*   [ ] Loot category icons are completed and imported into the project.
*   [ ] 4 time period icons are completed and imported into the project.
*   [ ] Condition icons for burning, stunned, and sleeping are completed and imported into the project.
*   [ ] All icons strictly follow the required color rules regarding magic and gold.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
