using GoogleSheet.Protocol.v2.Req;
using Minefarm.Entity.Actor.Monster.Controller;
using Minefarm.Entity.Actor.Player;
using Minefarm.Entity.DropItem;
using Minefarm.InGame;
using Minefarm.Item;
using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Entity.Actor.Monster
{
    public class MonsterController : ActorController
    {
        public UnityEvent<ActorModel> onFindTarget = new();

        public ActorModel target;
        public float distanceEpsilon;
        public float maxMoveRotatingAngle;

        public float distanceFollwoingTarget;
        public float delayFindTarget;
        public float delayStartMissingTarget;
        public float delayMissingTarget;

        private MonsterMoveController moveController;

        private PlayerModel player { get => GameManager.ins.player; }

        public override void Awake()
        {
            base.Awake();

            moveController = new(this);

            actorModel.onDead.AddListener(() =>
            {
                var dropItems = MonsterDB.GetDropItems(actorModel.entityID);
                foreach(var pair in dropItems)
                {
                    ItemID itemID = pair.Key;
                    int cnt = pair.Value;

                    Spawner.DropItem(actorModel.centerPosition, itemID, cnt);
                }
                Destroy(gameObject, 1f);
            });

            AddReactiveForFindTarget();
            AddReactiveForMissingTarget();
        }

        public virtual void Update()
        {
            moveController.Update();
        }

        private void AddReactiveForFindTarget()
        {
            float timeFindTarget = delayFindTarget;
            this.UpdateAsObservable()
                .Subscribe(_ => timeFindTarget -= Time.deltaTime);

            this.UpdateAsObservable()
                .Where(_ => target == null)
                .Where(_ => timeFindTarget <= 0.0f)
                .Where(_ =>
                {
                    timeFindTarget = delayFindTarget;

                    float sqr = distanceFollwoingTarget;
                    sqr *= sqr;
                    return (player.transform.position - transform.position).sqrMagnitude
                    < sqr;
                })
                .Where(_ =>
                {
                    RaycastHit hit;
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Ray ray = new Ray(actorModel.centerPosition + 0.5f* direction, direction);

                    Physics.Raycast(ray, out hit, distanceFollwoingTarget);
                    Debug.DrawRay(ray.origin, ray.direction*distanceFollwoingTarget, Color.red, 5f);
                    if (hit.transform == null) return false;

                    if (hit.transform.CompareTag("Player"))
                        return true;
                    return false;
                })
                .Subscribe(_ =>
                {
                    onFindTarget.Invoke(player);
                    target = player;
                });
        }

        private void AddReactiveForMissingTarget()
        {
            float timeStartMissingTarget = delayStartMissingTarget;
            this.UpdateAsObservable()
                .Where(_ => target != null)
                .Subscribe(_ => timeStartMissingTarget -= Time.deltaTime);

            float timeMissingTarget = delayMissingTarget;
            this.UpdateAsObservable()
                .Where(_ => target != null)
                .Where(_ => timeStartMissingTarget <= 0.0f)
                .Where(_ =>
                {
                    RaycastHit hit;
                    Vector3 forward = actorModel.body.forward;
                    Ray ray = new Ray(transform.position + 0.5f * forward, forward);

                    Physics.Raycast(ray, out hit, distanceFollwoingTarget);
                    if (hit.transform == null) return false;

                    if (hit.transform.CompareTag("Player"))
                        return true;
                    return false;
                })
                .Subscribe(_ =>
                {
                    timeStartMissingTarget = delayStartMissingTarget;
                    timeMissingTarget = delayMissingTarget;
                });
            this.UpdateAsObservable()
                .Where(_ => target != null)
                .Where(_ => timeStartMissingTarget <= 0.0f)
                .Subscribe(_ => timeMissingTarget -= Time.deltaTime);

            this.UpdateAsObservable()
                .Where(_ => target != null)
                .Where(_ => timeMissingTarget <= 0.0f)
                .Subscribe(_ => target = null);

            onFindTarget.AddListener(actor =>
            {
                timeStartMissingTarget = delayStartMissingTarget;
                timeMissingTarget = delayMissingTarget;
            });
        }
    }
}