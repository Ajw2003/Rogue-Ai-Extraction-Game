# Plan: Issue 14 - Health and Damage System Refactor

## Exhaustive Outline
Currently, players and enemies have health numbers hidden in the code, but there is no unified system to handle taking damage, nor is there any way to see health during gameplay. You cannot tell how much health you have left or if an enemy is close to being defeated. The goal of this task is to create a single, shared system for dealing and taking damage that works for both the player and enemies. We will also add health bars to the user interface and the game world so health is always clearly visible.

## Step by Step Execution Instructions

1.  **Unify the Damage Pipeline:**
    Create a new system or update `IHealth` to create a standard `TakeDamage` function. This function must be used universally. Ensure that both the `CastleGuard` (enemy script) and the `PlayerStats` (player script) use this exact same pathway to subtract health when attacked or affected by status effects like burning.

2.  **Implement Player Health UI:**
    Open the HUD system script (`RaidHudView.cs` or similar). Add a health bar or health numbers to the player's screen. Link this UI element directly to the `PlayerStats` health value so it updates immediately when the player takes damage or heals.

3.  **Implement Player Death:**
    Add logic to the player script so that when their health reaches zero, a "Game Over" or "Defeated" sequence triggers, rather than the game just continuing with negative health.

4.  **Implement Enemy Health Bars:**
    Create a world-space UI health bar above the enemies. Have the enemy script update this health bar based on its `CurrentHealth` and `MaxHealth` values. Alternatively, implement a physical stagger or damage-state system if world-space health bars are not desired, but health bars are the clearest option.

## Verification Steps
1.  Launch the game and look at your screen. Verify your health bar is visible and full.
2.  Find an enemy and look above their head. Verify their health bar is visible and full.
3.  Let the enemy attack you. Verify your health bar decreases and does not drop below zero.
4.  Allow your health to reach zero. Verify the game recognizes your defeat and triggers a game over state.
5.  Restart, find an enemy, and attack them. Verify their health bar decreases correctly based on the damage dealt.
6.  Continue attacking until their health reaches zero. Verify they are defeated.

## Completion Checks
*   [ ] A single, shared damage code pathway is used by all characters.
*   [ ] The player's current health is clearly visible on their screen.
*   [ ] The player can be defeated when their health reaches zero, triggering a failure state.
*   [ ] Enemy health is clearly visible in the game world, such as through a health bar above them.


## Technical Constraints
When executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in `docs/UnityConvention.md`. This includes using `m_camelCase` for private fields, Allman braces, `[SerializeField]` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle.
