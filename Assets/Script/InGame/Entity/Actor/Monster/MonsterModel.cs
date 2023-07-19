using Minefarm.Entity.Actor.FowardActionable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster
{
    public class MonsterModel : ActorModel
    {
        public void Awake()
        {
            base.Awake();

            fowardActionable = new MonsterFowardActionable(this);
        }
    }
}

