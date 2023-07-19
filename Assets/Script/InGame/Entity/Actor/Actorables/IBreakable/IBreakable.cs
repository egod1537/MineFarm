using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Interface
{
    public interface IBreakable
    {
        public bool Action(ActorModel target, int damage);
    }
}
