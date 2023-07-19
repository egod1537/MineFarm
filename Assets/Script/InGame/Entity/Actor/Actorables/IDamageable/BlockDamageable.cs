using Minefarm.Entity.Actor.Interface;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
namespace Minefarm.Entity.Actor.Damageable
{
    public class BlockDamageable : Actorable, IDamageable
    {
        public BlockDamageable(ActorModel actor) : base(actor)
        {
        }

        public bool Damage(ActorModel target, int damage,
            out int retDamage, out bool isCritical)
        {
            isCritical = false;
            retDamage = damage;
            actor.hp -= retDamage;

            return true;
        }
    }
}