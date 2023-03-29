using UnityEngine;

namespace Final_Survivors.Enemies
{
    public abstract class Tree : MonoBehaviour
    {
        protected Node _root { get; private set; }
        [SerializeField] public float tickRate;

        protected void Awake()
        {
            _root = SetupTree();    
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(Evaluate), 0, tickRate);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Evaluate));
        }

        protected abstract Node SetupTree();

        protected abstract void Evaluate();
    }
}
