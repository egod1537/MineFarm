using GoogleSheet.Protocol.v2.Req;
using Minefarm.InGame;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Entity.DropItem
{
    public class DropItemController : MonoBehaviour
    {
        const float DISTANCE_PICKUP = 1.0f;
        const float SQL_DISTANCE_PICKUP = DISTANCE_PICKUP* DISTANCE_PICKUP;

        const float DISTANCE_MAGNETIC = 3.0f;
        const float SQL_DISTANCE_MAGNETIC = DISTANCE_MAGNETIC* DISTANCE_MAGNETIC;

        const float POWER_MAGNETIC = 2.0f;

        private DropItemModel _model;
        public DropItemModel model
        {
            get => _model ??= GetComponent<DropItemModel>();
        }

        public void Awake()
        {
            SubcribeDelay();
        }

        float delayPickUp = 1.5f;
        private void SubcribeDelay()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => delayPickUp -= Time.deltaTime);

            this.UpdateAsObservable()
                .Where(_ => delayPickUp <= 0f)
                .Select(_ => GameManager.ins.player)
                .Where(player =>
                {
                    Vector3 diff = player.centerPosition - transform.position;
                    return diff.sqrMagnitude < SQL_DISTANCE_PICKUP;
                })
                .Subscribe(player =>
                {
                    if(player.inventory.controller.Add(model.item))
                        Destroy(this.gameObject);
                });

            this.UpdateAsObservable()
                .Where(_ => delayPickUp <= 0f)
                .Select(_ => GameManager.ins.player)
                .Where(player =>
                {
                    Vector3 diff = player.centerPosition - transform.position;
                    return diff.sqrMagnitude < SQL_DISTANCE_MAGNETIC;
                })
                .Subscribe(player =>
                {
                    transform.position =
                        Vector3.Lerp(transform.position, 
                        player.centerPosition, 
                        POWER_MAGNETIC * Time.deltaTime);
                });
        }
    }
}