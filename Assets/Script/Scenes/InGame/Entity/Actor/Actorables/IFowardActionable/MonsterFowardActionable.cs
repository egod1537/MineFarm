using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.FowardActionable
{
    public class MonsterFowardActionable : Actorable, IFowardActionable
    {
        public MonsterFowardActionable(ActorModel actor) : base(actor)
        {
        }

        public bool Action(EntityModel target)
        {
            throw new System.NotImplementedException();
        }
    }
}