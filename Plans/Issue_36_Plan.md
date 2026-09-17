# Plan: Issue 36 - Era-specific loot catalogue is unauthored - five generic pieces stand in for the pitch's four eras

## Exhaustive Outline
Currently, the game uses five basic, generic loot items for every time period. This task requires creating new, unique loot items that match the four specific historical eras from the design document (Bronze, High Medieval, Late Medieval, and Powder). We also need to create a large, special item called the Altarpiece that requires two people to carry. Finally, we need to ensure that the value of these items increases as players get closer to the important rooms in the castle, like the crypt or chapel.

## Step by Step Execution Instructions

1.  **Create Bronze Era Loot:**
    Create the visual models and setup the items for ingots, faience, ceremonial bronze, and sealed amphorae. Save these in the project.
2.  **Create High Medieval Era Loot:**
    Create the visual models and setup the items for reliquaries, altar plate, illuminated psalters, and coin. Save these in the project.
3.  **Create Late Medieval Era Loot:**
    Create the visual models and setup the items for Burgundian plate, tapestry, banking ledgers, and jewels. Save these in the project.
4.  **Create Powder Era Loot:**
    Create the visual models and setup the items for cabinets of curiosity, mirrors, astrolabes, and silver services. Save these in the project.
5.  **Create the Altarpiece:**
    Build a large visual model for the Altarpiece. Set its properties so that it requires two characters to carry it.
6.  **Assign Values to Loot:**
    Update the settings for all new items so their reward values increase based on how close they are placed to the crypt or chapel areas.

## Verification Steps
1.  Start a game in the Bronze era and check that only Bronze era items appear.
2.  Repeat the first step for the High Medieval, Late Medieval, and Powder eras to ensure only their specific items appear.
3.  Find the Altarpiece in the game and confirm it takes two characters to pick it up and move it.
4.  Walk through a level and confirm that the loot found deeper in the level near the crypt or chapel is worth more than the loot found near the entrance.

## Completion Checks
*   [ ] The Bronze, High Medieval, Late Medieval, and Powder eras each have their own unique set of loot items.
*   [ ] The five generic placeholder items are no longer the only loot used.
*   [ ] The Altarpiece exists as a large item that requires two people to carry.
*   [ ] Loot reward values increase correctly as players get closer to the crypt or chapel zones.
