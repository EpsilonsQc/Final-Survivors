using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class RangedAttack : Node
    {
        private RangedEnemy instance;
        private LayerMask mask;

        public RangedAttack(Transform transform)
        {
            instance = transform.GetComponent<RangedEnemy>(); 
            mask.value = (1 << 3);
        }

        public override NodeState Evaluate()
        {
            if (instance != null) // Projectile bug 
            {
                Vector3 endPosition = new Vector3(instance.playerTransform.position.x, instance.transform.position.y, instance.playerTransform.position.z);
                if (Vector3.Distance(instance.transform.position, instance.playerTransform.position) <= instance.attackRange && !Physics.Linecast(instance.transform.position, endPosition, mask))
                {
                    instance.Agent.speed = 0f;

                    if (instance.Agent.isOnNavMesh)
                        instance.Agent.isStopped = true;

                    if (instance.AttackTimer >= instance.attackSpeed)
                    {

                        if (!instance.animator.GetBool("isAttacking"))
                        {

                            foreach (AnimatorControllerParameter param in instance.animator.parameters)
                            {
                                instance.animator.SetBool(param.name, false);
                            }

                            instance.animator.SetBool("isAttacking", true);
                            instance.isAttacking = true;
                        }

                        return NodeState.SUCCESS;
                    }

                    return NodeState.RUNNING;
                }
            }

            return NodeState.FAILURE;
        }
    }
}
