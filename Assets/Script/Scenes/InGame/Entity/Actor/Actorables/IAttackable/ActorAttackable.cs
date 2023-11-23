using Minefarm.Entity.Actor.Interface;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;

namespace Minefarm.Entity.Actor.Attackable
{
    public class ActorAttackable : Actorable, IAttackable
    {
        public ActorAttackable(ActorModel actor) : base(actor)
        {
        }

        public bool Attack(ActorModel target, int damage)
        {
            if (!CanAttack(target)) return false;
            target.actorController.Damge(actor, damage);
            return true;
        }

        public bool CanAttack(ActorModel target)
        {
            return true;
        }

        public bool CanTarget(ActorModel target)
        {
            return target != null && actor.group != target.group;
        }
    }
}

