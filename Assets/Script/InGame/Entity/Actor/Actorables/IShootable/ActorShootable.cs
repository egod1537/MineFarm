using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Entity.Bullet;

namespace Minefarm.Entity.Actor.Shootable
{
    public class ActorShootable : Actorable, IShootable
    {
        private float shootDelay;

        public ActorShootable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => shootDelay -= Time.deltaTime);
        }

        public bool CanShoot()
        {
            return shootDelay <= 0f;
        }

        public bool Shoot(Vector3 foward)
        {
            if (!CanShoot()) return false;
            Bulleter.ShotBullet(
                actor,
                actor.bulletModel,
                actor.centerPosition,
                actor.body.transform.forward,
                actor.calculatedAttackRange,
                actor.calculatedAttack,
                actor.bulletSpeed);
            shootDelay = 1.0f / actor.calculatedAttackSpeed;
            return true;
        }
    }
}