namespace StateMachine.States
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Exit()
        {
            // Don't force state transfer, just allow it to happen.
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
