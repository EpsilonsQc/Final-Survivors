using Final_Survivors.Core;
using Final_Survivors.Enemies;
using Final_Survivors.Observer;
using Final_Survivors.Player;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Final_Survivors.UI
{
    public class FloatingDamages : MonoBehaviour, IObserver
    {
        private Subject subject;
        private Enemy enemy;
        private PlayerManager player;
        private TextMeshPro damage;
        [SerializeField] private float hideTime = 0.3f;
        private MeshRenderer meshRenderer;
        private Transform mainCamTransform;
        [SerializeField] private Color minColor = Color.white;
        [SerializeField] private Color normalColor =  Color.yellow;
        [SerializeField] private Color maxColor = Color.red;

        void Awake()
        {
            subject = GetComponentInParent<Subject>();
            enemy = GetComponentInParent<Enemy>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            damage = GetComponentInChildren<TextMeshPro>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            mainCamTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        private void Update()
        {
            if (meshRenderer.enabled)
                RotateCanvas();
        }

        private void OnEnable()
        {
            subject = GetComponentInParent<Subject>();
            subject.AddObserver(this);
        }

        private void OnDisable()
        {
            subject.RemoveObserver(this);
        }

        public void OnNotify(Events action)
        {
            if (action == Events.MISS)
            {
                StopCoroutine(nameof(HideFloatingNumber));

                damage.text = "Miss";
                damage.fontSize = 3;
                damage.color = minColor;
                meshRenderer.enabled = true;
                StartCoroutine(nameof(HideFloatingNumber));
            }

            if (action == Events.LOW_DMG)
            {
                StopCoroutine(nameof(HideFloatingNumber));

                if (enemy != null)
                    damage.text = enemy.TakenDamage.ToString();
                else
                    damage.text = player.TakenDamage.ToString();

                damage.color = minColor;
                damage.fontSize = 3;
                meshRenderer.enabled = true;
                StartCoroutine(nameof(HideFloatingNumber));
            }

            if (action == Events.NORMAL_DMG)
            {
                StopCoroutine(nameof(HideFloatingNumber));

                if (enemy != null)
                    damage.text = enemy.TakenDamage.ToString();
                else
                    damage.text = player.TakenDamage.ToString();

                damage.color = normalColor;
                damage.fontSize = 4;
                meshRenderer.enabled = true;
                StartCoroutine(nameof(HideFloatingNumber));
            }

            if (action == Events.CRIT_DMG)
            {
                StopCoroutine(nameof(HideFloatingNumber));

                if (enemy != null)
                    damage.text = enemy.TakenDamage.ToString();
                else
                    damage.text = player.TakenDamage.ToString();

                damage.color = maxColor;
                damage.fontSize = 6;
                meshRenderer.enabled = true;
                StartCoroutine(nameof(HideFloatingNumber));
            }
        }

        private IEnumerator HideFloatingNumber()
        {
            yield return new WaitForSeconds(hideTime);
            meshRenderer.enabled = false;
        }

        private void RotateCanvas()
        {
            transform.forward = mainCamTransform.forward;
        }
    }
}
