using UnityEngine.AI;
using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class Dead : Node
    {
        private Enemy instance;

        public Dead(Transform transform)
        {
            instance = transform.GetComponent<Enemy>();
        }
        public override NodeState Evaluate()
        {
            if(instance.IsDead)
            {
                if (!instance.animator.GetBool("isDead"))
                {
                    foreach (AnimatorControllerParameter param in instance.animator.parameters)
                    {
                        instance.animator.SetBool(param.name, false);
                    }
                    instance.animator.SetBool("isDead", true);

                    instance.Agent.speed = 0;

                    if (instance.Agent.isOnNavMesh)
                        instance.Agent.isStopped = true;

                    instance.Agent.enabled = false;
                }

                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
}
