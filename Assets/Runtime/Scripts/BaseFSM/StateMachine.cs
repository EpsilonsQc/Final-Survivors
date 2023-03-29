using UnityEngine;

namespace Final_Survivors
{
    public abstract class StateMachine : MonoBehaviour
    {
        private BaseState currentState;

        void FixedUpdate()
        {
            currentState?.OnUpdate();
        }

        public void ChangeState(BaseState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}
