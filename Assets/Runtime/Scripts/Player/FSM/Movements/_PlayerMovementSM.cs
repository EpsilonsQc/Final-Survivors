using UnityEngine;

namespace Final_Survivors.Player
{
    public class _PlayerMovementSM : StateMachine
    {
        public PlayerController PController { get; private set; }
        public PlayerManager playerMovement { get; private set; }
        public Animator Animator { get; private set; }
        public Vector2 moveInputValue { get; set; }

        void Awake()
        {
            moveInputValue = Vector2.zero;
            playerMovement = GetComponent<PlayerManager>();
            PController = GetComponent<PlayerController>();
            Animator = GetComponent<Animator>();

            ChangeState(new Idle(this));
        }
    }
}
