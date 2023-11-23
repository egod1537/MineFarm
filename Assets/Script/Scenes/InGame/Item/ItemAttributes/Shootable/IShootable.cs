using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item.Shootable
{
    public abstract class IShootable
    {
        public ActorModel actor { get; private set; }
        public IShootable(ActorModel actor)
        {
            SetActor(actor);
            this.actor = actor;
        }
        public virtual void SetActor(ActorModel actor)
        {
            this.actor = actor;
        }
        public abstract bool CanShoot();

        public abstract bool Shoot(Vector3 foward);
    }
}