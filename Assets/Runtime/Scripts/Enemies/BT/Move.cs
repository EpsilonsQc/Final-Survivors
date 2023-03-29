using UnityEngine;
using UnityEngine.AI;

namespace Final_Survivors.Enemies
{
    public class Move : Node
    {
        private Enemy instance;
        NavMeshPath navMeshPath = new NavMeshPath();

        public Move(Transform transform)
        {
            instance = transform.GetComponent<Enemy>();
        }

        public override NodeState Evaluate()
        {
            
            instance.Agent.enabled = true;
            instance.Agent.speed = instance.moveSpeed;

            // Tweek to correct PathComplete bug
            if (!instance.Agent.hasPath && instance.Agent.pathStatus == NavMeshPathStatus.PathComplete && instance.animator.GetBool("isMoving"))
            {
                
                instance.Agent.enabled = false;
                instance.Agent.enabled = true;
            }

            if (Vector3.Distance(instance.transform.position, instance.playerTransform.position) > instance.attackRange)
            {
                ChooseNewPath();

                if (navMeshPath.status == NavMeshPathStatus.PathComplete || navMeshPath.status == NavMeshPathStatus.PathPartial)
                {
                    if (!instance.animator.GetBool("isMoving"))
                    {
                        foreach (AnimatorControllerParameter param in instance.animator.parameters)
                        {
                            instance.animator.SetBool(param.name, false);
                        }
                        instance.animator.SetBool("isMoving", true);
                    }

                    if (instance.Agent.isOnNavMesh)
                    {
                        instance.Agent.path = navMeshPath;
                        instance.Agent.isStopped = false;
                    }

                    return NodeState.SUCCESS;
                }
                return NodeState.RUNNING;
            }
            
            return NodeState.FAILURE;
        }
        private void ChooseNewPath()
        {
            if (instance.Agent.isOnNavMesh)
                instance.Agent.CalculatePath(instance.playerTransform.position, navMeshPath);
        }
    }
}
