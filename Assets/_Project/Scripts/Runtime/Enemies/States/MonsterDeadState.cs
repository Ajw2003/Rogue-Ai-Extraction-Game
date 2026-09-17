namespace StateMachine.States
{
    public class MonsterDeadState : MonsterState
    {
        public MonsterDeadState(MonsterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine.DestroySelf();
        }

        public override void Update()
        {
            // Nothing should happen once dead.
        }

        public override void Exit()
        {
        }
    }
}
