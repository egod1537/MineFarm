using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Entity.Bullet;
using Minefarm.Entity.Actor;
using Minefarm.Entity;

namespace Minefarm.Item.Shootable
{
    public class DefaultShootable : IShootable
    {
        private float shootDelay;

        public DefaultShootable(ActorModel actor) : base(actor)
        {

        }

        public override void SetActor(ActorModel actor)
        {
            base.SetActor(actor);
            actor.UpdateAsObservable()
                .Subscribe(_ => shootDelay -= Time.deltaTime);
        }
        public override bool CanShoot()
        {
            return shootDelay <= 0f;
        }

        public override bool Shoot(Vector3 foward)
        {
            if (!CanShoot()) return false;
            Spawner.ShotBullet(
                actor,
                actor.stats.bulletModel,
                actor.centerPosition,
                actor.body.transform.forward,
                actor.stats.calculatedAttackRange,
                actor.stats.calculatedAttack);
            shootDelay = 1.0f / actor.stats.calculatedAttackSpeed;
            return true;
        }
    }
}