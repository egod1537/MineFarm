using Minefarm.Entity.Actor.Interface;
using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
namespace Minefarm.Entity.Actor.Breakable
{
    public class PlayerDigable : Actorable, IDigable
    {
        private float breakDelay;
        public PlayerDigable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => breakDelay -= Time.deltaTime);
        }

        public bool Dig(BlockModel target, int damage)
        {
            if (!CanDig(target)) return false;
            target.actorController.Damge(actor, damage);
            breakDelay = 1.0f / actor.calculatedAttackSpeed;
            return true;
        }

        public bool CanDig(BlockModel target)
        {
            return breakDelay <= 0f && BlockDB.IsBreak(target.blockID);
        }
    }
}

