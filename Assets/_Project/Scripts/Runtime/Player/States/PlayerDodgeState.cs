using UnityEngine;

namespace StateMachine.States
{
    public class PlayerDodgeState : PlayerState
    {
        public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            Vector3 inputDirection = new Vector3(_stateMachine.MovementDirection.x, 0, _stateMachine.MovementDirection.y);
            if (inputDirection.sqrMagnitude > 1f)
            {
                inputDirection.Normalize();
            }

            // Transform direction from local (player-relative) to world space. With standard
            // world gravity the body stays upright, so local axes align with world axes.
            Vector3 dodgeDirection = _stateMachine.transform.TransformDirection(inputDirection);
            _stateMachine._rb.AddForce(dodgeDirection * _stateMachine.DodgeForce, ForceMode.Impulse);
        }

        public override void FixedUpdate()
        {
            if (_stateMachine._rb.linearVelocity.magnitude < 1.0f)
            {
                Exit();
            }
        }

        public override void Exit()
        {
            if (_stateMachine.MovementDirection.sqrMagnitude > 0.1f)
            {
                _stateMachine.ChangeState(_stateMachine.WalkState);
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }
}
