# Plan: Issue 35 - Era-specific hazards from the pitch are entirely unimplemented

## Exhaustive Outline
The original game pitch promised unique dangers for each historical time period. These include faster fire in the Bronze Age, awkward clockwise stairs in the High Medieval era, paired guards in the Late Medieval era, and explosive magazines in the Powder Age. Currently, these unique dangers are missing from the game rules and documentation. The goal of this task is to design, implement, and document these specific hazards so each time period feels distinct and dangerous.

## Step by Step Execution Instructions

1.  **Design Era Hazards:** Decide exactly how each of the four era hazards will work within the game rules.
2.  **Update Castle Rules:** Add the new rules for clockwise stairs and explosive magazines to the castle documentation.
3.  **Update Alarm Rules:** Add the new rules for faster fire and paired guards to the alarm and guard documentation.
4.  **Review System Hooks:** Ensure the game systems can trigger these specific rules based on the current era.

## Verification Steps
1.  Play a Bronze Age level and verify that fire spreads faster than normal.
2.  Play a High Medieval level and check that navigating stairs has the intended disadvantage.
3.  Play a Late Medieval level and confirm that guards appear and search in pairs.
4.  Play a Powder Age level and test that fire near a magazine causes a massive explosion.

## Completion Checks
*   [ ] Bronze Age fire hazard is implemented and documented.
*   [ ] High Medieval stair hazard is implemented and documented.
*   [ ] Late Medieval guard hazard is implemented and documented.
*   [ ] Powder Age magazine hazard is implemented and documented.
*   [ ] All new rules are documented in the appropriate system files.
