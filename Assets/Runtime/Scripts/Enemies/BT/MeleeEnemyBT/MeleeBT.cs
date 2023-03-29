using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class MeleeBT : Tree
    {

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Dead(transform),
                new Idle(transform),
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
