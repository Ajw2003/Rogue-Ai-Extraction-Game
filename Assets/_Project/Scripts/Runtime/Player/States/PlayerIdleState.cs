using Code.Scripts.EventSystems;
using StateMachine;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        EventManager.Instance?.Publish(new PlayerIdleEvent());
    }
}
