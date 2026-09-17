# Plan: Issue 10 - Enemy Animations

## Exhaustive Outline
Currently, enemies in the game do not have any animations. When they move toward the player, they slide across the floor in a stiff, default pose. They do not swing their weapons or fall over when defeated. The goal of this task is to add basic animations to all enemies. This includes idle breathing, walking, attacking, and dying. We will also update the system that creates the enemy game objects so it automatically attaches the required animation components.

## Step by Step Execution Instructions

1.  **Acquire and Prepare Animations:**
    Obtain basic animation clips for idle, walk/run, attack, and death. Ensure these animations work with the generic skeleton used by the enemy models.

2.  **Create Animator Controllers:**
    Create an `AnimatorController` in Unity for the enemies. Set up the animation states (Idle, Locomotion, Attack, Death) and create the transitions between them. Use parameters like a `Speed` float and an `IsDead` boolean to control these transitions.

3.  **Update the Enemy Prefab Tool:**
    Open the `EnemyPrefabForge` script. Modify this tool so that when it builds an enemy, it automatically adds an `Animator` component and assigns the correct `AnimatorController` to it.

4.  **Connect Code to Animations:**
    Open the script that controls enemy behavior, such as `CastleGuard`. In the `Update` method or using the `StateChanged` event, send data to the `Animator`. Send the current movement speed from the navigation system to the `Speed` parameter. Trigger the attack animation when the enemy attacks, and set the `IsDead` parameter when the enemy is defeated.

## Verification Steps
1.  Run the `EnemyPrefabForge` tool to rebuild the enemy models.
2.  Launch the game and find an enemy.
3.  Observe the enemy from a distance to verify it plays an idle animation instead of standing completely frozen.
4.  Get the enemy's attention and watch it move toward you. Verify it plays a walking or running animation.
5.  Let the enemy reach you and attack. Verify it plays an attacking animation.
6.  Defeat the enemy. Verify it plays a death animation instead of just disappearing.

## Completion Checks
*   [ ] All enemy models have an `Animator` component attached.
*   [ ] Enemies play an idle animation when standing still.
*   [ ] Enemies play a movement animation that matches their movement speed.
*   [ ] Enemies play an attack animation when striking.
*   [ ] Enemies play a death animation when defeated.
*   [ ] The prefab forge tool automatically sets up these animation components without manual intervention.
