# Plan: Issue 12 - Combat Damage Visual Feedback

## Exhaustive Outline
Currently, there are no visual signs when the player attacks an enemy or when the player takes damage. The health numbers change in the background, but the game does not flash or show any impact effects. This makes fighting confusing. The goal of this task is to add clear visual signals for both dealing and taking damage. The player should see a flash on the screen when hurt, and see enemies flash or show a hit marker when struck. Defeating an enemy must also look different from just hurting them.

## Step by Step Execution Instructions

1.  **Add Player Damage Feedback:**
    Create a screen overlay (such as a red vignette or flash effect) in the UI. Open the script that handles player health. When the player loses health, trigger this overlay to flash briefly.

2.  **Add Enemy Damage Feedback:**
    Open the `CastleGuard` script or the health system handling enemies. Create a system to briefly change the enemy's color or material to white or red (a "hit flash") when they take damage.
    Alternatively, or additionally, add a small UI hit marker (like an "X" near the crosshair) that appears when the player successfully damages an enemy.

3.  **Differentiate Death from Damage:**
    Ensure the death state provides a different visual cue than standard damage. This could be a larger final flash, a specific particle effect, or dissolving the enemy model.

4.  **Connect to Existing Health Systems:**
    Ensure these visual effects are triggered by the existing `IHealth` system so they automatically work whenever health is reduced by any source, including status effects like burning.

## Verification Steps
1.  Launch the game and find an enemy.
2.  Allow the enemy to attack the player. Verify the screen flashes or shows a red border to indicate pain.
3.  Attack the enemy. Verify the enemy flashes, or a hit marker appears on screen to confirm the hit.
4.  Continue attacking the enemy until it runs out of health. Verify the final hit looks noticeably different from a normal hit, confirming the enemy is defeated.

## Completion Checks
*   [ ] The player's screen flashes or changes color when they take damage.
*   [ ] Enemies flash or the player sees a hit marker when successfully dealing damage.
*   [ ] Defeating an enemy produces a different visual effect than a regular damage hit.
*   [ ] Visual feedback works reliably through the existing health system.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
