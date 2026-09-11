using UnityEngine;

// Base class for all player state machine states - walking, jumping, in an inventory UI, etc.
namespace StateMachine
{
    public class PlayerState : IState
    {
        protected PlayerStateMachine _stateMachine;

        public PlayerState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
        }

        public virtual void HandleMovement()
        {
        }
    }
}
