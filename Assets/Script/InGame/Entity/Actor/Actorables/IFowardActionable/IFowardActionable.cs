using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.FowardActionable
{
    public interface IFowardActionable 
    {
        public bool Action(EntityModel target);
    }
}