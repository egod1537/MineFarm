using Minefarm.Entity.Actor.Interface;
using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Actor;

namespace Minefarm.Item.Digable
{
    public class DefaultDigable : IDigable
    {
        private float breakDelay;

        public DefaultDigable(ActorModel actor) : base(actor)
        {
        }

        public override void SetActor(ActorModel actor)
        {
            base.SetActor(actor);
            actor.UpdateAsObservable()
                .Subscribe(_ => breakDelay -= Time.deltaTime);
        }

        public override bool CanDig(BlockModel target)
        {
            return breakDelay <= 0f && BlockDB.IsBreak(target.blockID);
        }

        public override bool Dig(BlockModel target, int damage)
        {
            if (!CanDig(target)) return false;
            target.actorController.Damge(actor, damage);
            breakDelay = 1.0f / actor.stats.calculatedAttackSpeed;
            return true;
        }
    }
}

