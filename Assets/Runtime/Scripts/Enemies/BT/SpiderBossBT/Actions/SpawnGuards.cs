using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class SpawnGuards : Node
    {
        private SpiderBoss instance;

        private LayerMask mask;

        public SpawnGuards(Transform transform)
        {
            instance = transform.GetComponent<SpiderBoss>();
            mask.value = (1 << 3);
        }

        public override NodeState Evaluate()
        {
            if (instance != null) // Projectile bug 
            {
                if (!instance.IsSpawnReady)
                {
                    return NodeState.FAILURE;
                }

                if (instance.isAttacking)
                {
                    return NodeState.RUNNING;
                }

                Vector3 endPosition = new Vector3(instance.playerTransform.position.x, instance.transform.position.y, instance.playerTransform.position.z);
                if (instance.Health <= instance.MaxHealth * instance.SpawnAtHPPercent && instance.IsSpawnReady)
                {
                    instance.Agent.speed = 0f;

                    if (instance.Agent.isOnNavMesh)
                    {
                        instance.Agent.isStopped = true;
                    }

                    if (!instance.animator.GetBool("isSpawnGuards"))
                    {
                        foreach (AnimatorControllerParameter param in instance.animator.parameters)
                        {
                            instance.animator.SetBool(param.name, false);
                        }

                        instance.animator.SetBool("isSpawnGuards", true);
                        instance.isAttacking = true;
                    }

                    return NodeState.SUCCESS;
                }
            }

            return NodeState.FAILURE;
        }
    }
}
