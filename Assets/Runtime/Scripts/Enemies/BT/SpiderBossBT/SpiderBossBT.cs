using System.Collections.Generic;

namespace Final_Survivors.Enemies
{
    public class SpiderBossBT : Tree
    {
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
        {
            new Dead(transform),
            new Idle(transform),
            new SpawnGuards(transform),
            new ShootWebBall(transform),
            new MeleeAttack(transform),
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
