using UnityEngine;

namespace StateMachine
{
    public abstract class BaseStateMachine : MonoBehaviour
    {
        protected IState CurrentState { get; set; }
        protected string CurrentStateName;

        public virtual void ChangeState(IState newState)
        {
            if (newState == CurrentState)
                return;

            CurrentState = newState;
            CurrentState?.Enter();
            CurrentStateName = CurrentState?.ToString();
        }

        public virtual void Update()
        {
            CurrentState?.Update();
        }

        public virtual void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}
