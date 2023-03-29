using UnityEngine;
using Final_Survivors.Enemies;
using Final_Survivors.Audio;

namespace Final_Survivors
{
    public class HiddenRoomCollision : MonoBehaviour
    {
        [SerializeField] GameObject radioPrefab;
        [SerializeField] GameObject specialCrate;
        [SerializeField] GameObject enemy;
        [SerializeField] GameObject fence;

        private AudioSource source;
        Enemy enemyHealth;
        private bool enemyDead;
        private SoundBank soundBank;

        private void Start()
        {
            source = enemy.GetComponent<AudioSource>();
            enemyHealth = enemy.GetComponent<Enemy>();
            enemy.SetActive(false);
            enemyDead = false;
            soundBank = GameObject.Find("SoundBank").GetComponent<SoundBank>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !enemyDead)
            {
                enemy.SetActive(true);
                enemyDead = true;
                radioPrefab.SetActive(false);

                InvokeRepeating(nameof(CheckEnemyDead), 0, 0.2f);
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.CompareTag("Player"))
            {
                fence.transform.localPosition = new Vector3(fence.transform.localPosition.x, 0.5f, fence.transform.localPosition.z);
            }
        }

        private void CheckEnemyDead()
        {
            if (enemyDead)
            {
                if (enemyHealth.Health <= 0)
                {
                    SoundManager.PlaySound(ref soundBank.crates, source, source.volume / 3);
                    specialCrate.SetActive(true);
                    CancelInvoke(nameof(CheckEnemyDead));
                }
            }
        }
    }
}
