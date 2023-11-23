using Minefarm.Entity.Actor.Interface;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
namespace Minefarm.Entity.Actor.Damageable
{
    public class ActorDamageable : Actorable, IDamageable
    {
        const float POWER_KNOCKBACK = 3f;
        const float DELAY_KNOCKBACK = 0.5f;

        private float delayKnockback;

        public ActorDamageable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => delayKnockback -= Time.deltaTime);
        }

        public bool Damage(ActorModel target, int damage,
            out int retDamage, out bool isCritical)
        {
            isCritical = false;
            retDamage = actor.stats.FormulateDamage(target, damage, out isCritical);
            actor.hp -= retDamage;

            KnockBack(target);
            return true;
        }

        public bool IsKnockBack()
        {
            return delayKnockback <= 0f;
        }

        public bool KnockBack(ActorModel target)
        {
            if (!IsKnockBack()) return false;
            Vector3 dir = transform.position - target.transform.position;
            dir.Normalize();
            rigidbody.AddForce(dir*POWER_KNOCKBACK, ForceMode.Impulse);
            delayKnockback = DELAY_KNOCKBACK;
            return true;
        }
    }
}