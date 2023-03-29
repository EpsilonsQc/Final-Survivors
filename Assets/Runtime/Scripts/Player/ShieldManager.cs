using Final_Survivors.Environment;
using Final_Survivors.Player;
using UnityEngine;

namespace Final_Survivors
{
    public class ShieldManager : MonoBehaviour
    {
        [Header("Shield regeneration per second")]
        [SerializeField] private float shieldRegen = 1.0f;

        static private float _shieldRegen;
        private static ShieldManager _shieldManager = null;
        static private PlayerManager playerManager;

        private void Awake()
        {
            _shieldManager = this;
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            _shieldRegen = shieldRegen; // Set the shield regen value
        }

        private void Update()
        {
            if (EnvironmentState.GetIsDay() && !EnvironmentState.GetIsPause())
                ShieldRegeneration();
        }

        public static ShieldManager Instances
        {
            get
            {
                if (_shieldManager == null)
                {
                    _shieldManager = new ShieldManager();
                }

                return _shieldManager;
            }
        }

        static public void ShieldRegeneration()
        {
            if (playerManager.ShieldValue <= playerManager.ShieldMaxValue)
            {
                playerManager.ShieldValue += _shieldRegen * Time.deltaTime;
            }
        }

        public PlayerManager GetPlayerManager()
        {
            return playerManager;
        }
    }
}


