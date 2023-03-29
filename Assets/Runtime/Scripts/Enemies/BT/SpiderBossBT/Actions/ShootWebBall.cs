using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class ShootWebBall : Node
    {
        private SpiderBoss instance;

        private LayerMask mask;

        public ShootWebBall(Transform transform)
        {
            instance = transform.GetComponent<SpiderBoss>();
            mask.value = (1 << 3);
        }

        public override NodeState Evaluate()
        {
            if (instance != null) // Projectile bug 
            {
                if (!instance.IsWebBallReady)
                {
                    return NodeState.FAILURE;
                }

                if (instance.isAttacking)
                {
                    return NodeState.RUNNING;
                }

                Vector3 endPosition = new Vector3(instance.playerTransform.position.x, instance.transform.position.y, instance.playerTransform.position.z);
                if (Vector3.Distance(instance.transform.position, instance.playerTransform.position) <= instance.WebBallRange && !Physics.Linecast(instance.transform.position, endPosition, mask))
                {
                    instance.Agent.speed = 0f;

                    if (instance.Agent.isOnNavMesh)
                    {
                        instance.Agent.isStopped = true;
                    }

                    if (!instance.animator.GetBool("isWebBall"))
                    {
                        foreach (AnimatorControllerParameter param in instance.animator.parameters)
                        {
                            instance.animator.SetBool(param.name, false);
                        }

                        instance.animator.SetBool("isWebBall", true);
                        instance.isAttacking = true;
                    }

                    return NodeState.SUCCESS;
                }
            }

            return NodeState.FAILURE;
        }
    }
}
