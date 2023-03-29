using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class Idle : Node
    {
        private Enemy instance;
        
        public Idle(Transform transform)
        {
            instance = transform.GetComponent<Enemy>();
        }

        public override NodeState Evaluate()
        {
            if (instance.IsSleepMode)
            {
                if (!instance.animator.GetBool("isIdle"))
                {
                    instance.Agent.enabled = false;

                    if (instance.Agent.isOnNavMesh)
                        instance.Agent.isStopped = true;

                    foreach (AnimatorControllerParameter param in instance.animator.parameters)
                    {
                        instance.animator.SetBool(param.name, false);
                    }

                    instance.animator.SetBool("isIdle", true);
                }
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
}
