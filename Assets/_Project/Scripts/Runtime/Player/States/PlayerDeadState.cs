using UnityEngine.SceneManagement;

namespace StateMachine.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
