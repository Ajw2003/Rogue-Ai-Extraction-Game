# Plan: Issue 15 - Discoverable Loot Interaction

## Exhaustive Outline
Currently, loot items spawn in the world, but they are too small to easily see and they don't look like things you can interact with. The player receives no button prompt or highlight to indicate an object can be picked up. Because of this, players do not realize they can carry items. The goal of this task is to make loot highly visible, add a highlight effect to items when the player looks at them, and display an on-screen prompt showing the item's name and the button required to pick it up. Very heavy items must also clearly show that they require two players to carry.

## Step by Step Execution Instructions

1.  **Increase Loot Visibility:**
    Review the sizes of the five authored loot prefabs (CopperPot, SilverPlate, GoldenGoblet, HeavyChest, AncientRelic). Scale them up so they are clearly visible from across a room.

2.  **Implement Object Highlighting:**
    Add a script or shader to the loot prefabs that creates a glowing outline or brightens the object's color. Open `LootInteractor.cs` and write logic so that when the player's aim is focused on the object, this highlight effect turns on. When the player looks away, turn it off.

3.  **Add Interaction Prompts:**
    Open the HUD script (like `RaidHudView.cs`). Add a text element to the screen that appears near the crosshair.
    When `LootInteractor` detects an object, read the object's name and its required interaction key. Update the HUD text to say something like "Press [E] to pick up Golden Goblet".

4.  **Indicate Heavy Items:**
    For items marked as heavy (like the HeavyChest), add specific text to the interaction prompt. The prompt must clearly state that two people are required, such as "Heavy Chest - Requires 2 Players".

## Verification Steps
1.  Launch the game and enter a room with loot.
2.  Look around. Verify the loot items are large enough to spot immediately.
3.  Walk up to a regular item and look directly at it. Verify the item highlights or glows.
4.  Verify a text prompt appears on screen with the correct button to press and the item's name.
5.  Look away from the item. Verify the highlight and the prompt disappear.
6.  Find a heavy item and look at it. Verify the prompt clearly states that two players are needed.

## Completion Checks
*   [ ] Loot items are scaled up and easily visible from a distance.
*   [ ] Loot items highlight or glow when the player looks at them.
*   [ ] An on-screen prompt tells the player what the item is and what button to press.
*   [ ] Heavy items explicitly state in their prompt that two players are required to carry them.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
