using System.Collections.Generic;

namespace Final_Survivors.Enemies
{
    public class RangedBT : Tree
    {
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Dead(transform),
                new Idle(transform),
                new RangedAttack(transform),
                new Move(transform)
            });

            return root;
        }

        protected override void Evaluate()
        {
            _root.Evaluate();
        }
    }
}
