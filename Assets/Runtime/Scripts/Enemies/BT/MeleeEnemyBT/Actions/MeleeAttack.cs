using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class MeleeAttack : Node
    {
        private Enemy instance;
        private LayerMask mask;

        public MeleeAttack(Transform transform)
        {
            instance = transform.GetComponent<Enemy>(); 
        }

        public override NodeState Evaluate()
        {
            if (instance.AttackTimer < instance.attackSpeed)
            {
                return NodeState.FAILURE;
            }

            if (instance.isAttacking)
            {
                return NodeState.RUNNING;
            }

            if (Vector3.Distance(instance.transform.position, instance.playerTransform.position) <= instance.attackRange )
            {
                instance.Agent.speed = 0f;

                if (instance.Agent.isOnNavMesh)
                    instance.Agent.isStopped = true;

                if (!instance.animator.GetBool("isAttacking"))
                {
                    foreach (AnimatorControllerParameter param in instance.animator.parameters)
                    {
                        instance.animator.SetBool(param.name, false);
                    }
                    instance.animator.SetBool("isAttacking", true);
                    instance.isAttacking = true;
                               
                    return NodeState.SUCCESS;
                }                             
            }

            return NodeState.FAILURE;
        }
    }
}
