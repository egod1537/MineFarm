using Minefarm.Entity.Actor.Interface;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
namespace Minefarm.Entity.Actor.Attackable
{
    public class ActorAttackable : Actorable, IAttackable
    {
        private float attackDelay;

        public ActorAttackable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => attackDelay -= Time.deltaTime);
        }

        public bool Attack(ActorModel target, int damage)
        {
            if (!CanAttack(target)) return false;
            target.actorController.Damge(actor, damage);
            attackDelay = 1.0f/ actor.calculatedAttackSpeed;
            return true;
        }

        public bool CanAttack(ActorModel target)
        {
            return attackDelay <= 0f;
        }
    }
}

