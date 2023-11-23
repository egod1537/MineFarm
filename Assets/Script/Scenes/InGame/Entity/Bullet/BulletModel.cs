using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity.Bullet
{
    /// <summary>
    /// Bullet은 Actor Model만 충돌을 허용한다.
    /// </summary>
    public class BulletModel : EntityModel

    {
        public ActorModel owner;

        public int damage;
        public float distance;
        public Vector3 direction;
        public float speed;

        public void Awake()
        {
            this.OnTriggerEnterAsObservable()
                .Select(collider => collider.gameObject)
                .Where(go => go != owner.gameObject)
                .Select(go => go.GetComponent<ActorModel>())
                .Where(actor => owner.attackable.CanTarget(actor))
                .Subscribe(actor =>
                {
                    if (!actor.isLive || actor.isDead)
                        return;

                    owner.actorController.Attack(actor, damage);
                    if (actor.isDead) owner.onKillEntity.Invoke(actor);
                    Destroy(this.gameObject);
                });

            this.OnTriggerEnterAsObservable()
                .Select(collider => collider.gameObject)
                .Where(go => go != owner.gameObject)
                .Select(go => go.GetComponent<ActorModel>())
                .Where(actor => actor is BlockModel)
                .Subscribe(_ => Destroy(this.gameObject));

            this.UpdateAsObservable()
                .Where(_ => distance <= 0f)
                .Subscribe(_ => Destroy(this.gameObject));
        }
        public void Update()
        {
            float movement = speed * Time.deltaTime;
            transform.position += direction * movement;
            distance -= movement;
        }
    }
}