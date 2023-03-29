using System.Collections.Generic;

namespace Final_Survivors.Enemies
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected List<Node> childrenList = new List<Node>();

        public Node()
        {
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                childrenList.Add(child);
            }
        }

        public virtual NodeState Evaluate()
        {
            return NodeState.FAILURE;
        }
    }
}
