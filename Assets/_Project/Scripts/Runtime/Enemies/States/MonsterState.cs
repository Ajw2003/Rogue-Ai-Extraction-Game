using StateMachine;

public class MonsterState : IState
{
    protected MonsterStateMachine _stateMachine;

    public MonsterState(MonsterStateMachine stateMachine)
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
}
