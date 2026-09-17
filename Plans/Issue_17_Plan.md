# Plan: Issue 17 - Era-Specific Art and Filtering

## Exhaustive Outline
The game allows the player to select from four different historical eras (Bronze Age, High Medieval, Late Medieval, Age of Powder) before starting a raid. However, only the High Medieval era actually has art, rooms, and enemies. If the player picks any other era, the game just loads the High Medieval content anyway. This breaks the illusion of time travel. The goal of this task is to fix this discrepancy. We will either temporarily disable the unfinished eras in the menu, or we will update the game's data systems so that each era loads its own specific set of rooms, enemies, and items.

## Step by Step Execution Instructions

1.  **Decide on Scope:**
    Determine if the team is ready to add distinct content for all four eras right now. If not, proceed to Step 2. If yes, proceed to Step 3.

2.  **Short-Term Fix (Disable Eras):**
    Open `LairScreen.cs`. Find the UI elements that let the player choose an era. Disable or lock the buttons for Bronze Age, Late Medieval, and Age of Powder. Add a small text label over them saying "Coming Soon" or "Locked". This prevents players from selecting empty eras.

3.  **Long-Term Fix (Implement Era Filtering):**
    Update the `CastleRoomRegistry`, `EnemyRoster`, and `RaidLootTable` data structures. Add an `Era` property to every room, enemy, and piece of loot.
    When `RaidDirector.StartRaid` is called, pass the chosen era into the generation systems. Modify the procedural generation and enemy spawning scripts so they only select items that match the chosen era.

4.  **Connect Era-Specific Weapons:**
    Review the weapons in `Assets/_Project/Prefabs/Weapons/` (like the BronzeSword or Matchlock). Ensure these weapons are added to the new era-filtered rosters so enemies and loot tables use the correct weapons for the chosen time period.

## Verification Steps
1.  Launch the game and navigate to the Lair screen.
2.  (If short-term fix): Verify you can only select High Medieval, and the other eras clearly say they are disabled or coming soon.
3.  (If long-term fix): Select a different era, like Age of Powder, and start the raid.
4.  Explore the generated castle. Verify that only Age of Powder rooms, enemies, and weapons appear. Verify no High Medieval content spawns.

## Completion Checks
*   [ ] The player can no longer select an era that has no content, OR
*   [ ] The procedural generator correctly filters rooms, enemies, and loot by the selected era.
*   [ ] The existing era-specific weapons (BronzeSword, Matchlock, etc.) are correctly assigned to their respective time periods.
*   [ ] The game never spawns mixed-era content in a single raid.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
