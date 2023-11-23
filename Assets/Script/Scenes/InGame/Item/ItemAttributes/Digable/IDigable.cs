using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Actor.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Item.Digable
{
    public abstract class IDigable
    {
        public ActorModel actor { get; private set; }
        public IDigable(ActorModel actor)
        {
            SetActor(actor);
        }

        public virtual void SetActor(ActorModel actor)
        {
            this.actor = actor;
        }

        public abstract bool CanDig(BlockModel target);

        public abstract bool Dig(BlockModel target, int damage);
    }
}

