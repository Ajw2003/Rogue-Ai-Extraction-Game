# Plan: Issue 54 - No performance budget or profiling pass exists for authored-art raids

## Exhaustive Outline
Currently, the game loads many visual elements like rooms, loot, and guards during a raid, but there is no limit set on how much computing power this is allowed to use. The goal of this task is to set a performance target for the game and measure if a typical raid meets that target. This will ensure the game continues to run smoothly as more visual elements are added.

## Step by Step Execution Instructions

1.  **Define Performance Targets:**
    Decide on the maximum allowed limits for frame processing time and graphics rendering commands during a typical raid.
2.  **Document the Targets:**
    Write down these limits clearly so they can be easily referenced by the team.
3.  **Run a Test Raid:**
    Play through a typical raid level that includes the standard amount of rooms, loot, and guards.
4.  **Record Performance:**
    Use performance measuring tools to record the actual frame processing time and graphics rendering commands during the test raid.
5.  **Compare Results:**
    Compare the recorded performance against the targets defined in the first step. Document whether the game passes or fails the budget.

## Verification Steps
1.  Play a standard raid in the game.
2.  Open the performance monitoring tools while playing.
3.  Confirm that the frame processing time and graphics rendering commands stay within the defined limits.

## Completion Checks
*   [ ] A performance target is defined for a typical raid.
*   [ ] A performance test is run and recorded during a typical raid.
*   [ ] The recorded test clearly shows whether the game passes or fails the performance targets.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
