using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Bullet
{
    public class BulletShooter : MonoBehaviour
    {
        private ActorModel _actorModel;
        public ActorModel actorModel { get => _actorModel ??= GetComponent<ActorModel>(); }

        public BulletModelType type;

        public Vector3 direction;
        public float speed = 1f;

        public void Shoot()
        {
            BulletModel bullet = Spawner.ShotBullet(
                actorModel,
                type, 
                transform.position, 
                direction, 
                actorModel.stats.calculatedAttackRange, 
                actorModel.stats.calculatedAttack, 
                speed);
        }
    }
}