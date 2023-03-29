using System.Collections.Generic;

namespace Final_Survivors.Enemies
{
    public class Sequence : Node
    {
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in childrenList)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        return NodeState.FAILURE;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                }
            }
            return NodeState.FAILURE;
        }
    }
}
