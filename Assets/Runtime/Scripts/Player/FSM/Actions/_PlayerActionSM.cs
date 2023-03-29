using UnityEngine;

namespace Final_Survivors.Player
{
    public class _PlayerActionSM : StateMachine
    {
        public PlayerController playerController { get; private set; }
        public Animator Animator { get; private set; }
        public CharacterController characterControler { get; private set; }
        public PlayerManager playerManager { get; private set; }

        void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerController = GetComponent<PlayerController>();
            Animator = GetComponent<Animator>();
            characterControler = GetComponent<CharacterController>();

            ChangeState(new IdleAction(this));
        }
    }
}
