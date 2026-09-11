using StateMachine;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Update()
    {
        if (_stateMachine.CameraTransform == null)
        {
            Debug.LogWarning("CameraTransform is not assigned in PlayerStateMachine. Cannot apply camera-relative movement.");
            return;
        }

        Vector3 up = _stateMachine.GravityReceiver.Up;
        Vector3 moveDirection = CameraRelativeInputOnGravityPlane(up);

        // Preserve the velocity component along up so this doesn't interfere with jumping/falling.
        float verticalSpeed = Vector3.Dot(_stateMachine._rb.linearVelocity, up);

        _stateMachine._rb.linearVelocity = (moveDirection * _stateMachine.walkSpeed) + (up * verticalSpeed);
    }

    private Vector3 CameraRelativeInputOnGravityPlane(Vector3 up)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(_stateMachine.CameraTransform.forward, up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(_stateMachine.CameraTransform.right, up).normalized;

        Vector3 moveDirection = (cameraForward * _stateMachine.MovementDirection.y) + (cameraRight * _stateMachine.MovementDirection.x);
        return moveDirection.normalized;
    }

    public override void Exit()
    {
        // Move to idle state when movement stops - TODO, not carried from ThirdPerson.
    }
}
