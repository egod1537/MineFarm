using Minefarm.Entity.Actor.Block;
using Minefarm.Map.Block;
using System;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Entity.Actor.Player
{
    public class PlayerController : ActorController
    {
        public PlayerModel playerModel { get=> actorModel as PlayerModel; }
        public override void Awake()
        {
            base.Awake();

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => base.Jump());

            this.UpdateAsObservable()
                .Select(_ =>
                {
                    float dx = Input.GetAxis("Horizontal"), dy = Input.GetAxis("Vertical");
                    Vector3 ret = new Vector3(dx + dy, 0f, dy - dx);
                    if (ret.sqrMagnitude > 1f) ret.Normalize();
                    return ret;
                })
                .Where(dir => !Mathf.Approximately(dir.sqrMagnitude, 0f))
                .Subscribe(dir => base.Move(dir));

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => FowardAction());

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(1))
                .Subscribe(_ => Dash());

            float delayDash = 0f;
            this.UpdateAsObservable()
                .Where(_ => playerModel.dashCount < PlayerModel.MAX_DASH_COUNT)
                .Subscribe(_ => delayDash -= Time.deltaTime);
            this.UpdateAsObservable()
                .Where(_ => delayDash <= 0f)
                .Subscribe(_ =>
                {
                    if(playerModel.dashCount < PlayerModel.MAX_DASH_COUNT)
                    {
                        playerModel.dashCount++;
                        playerModel.onDashCharged.Invoke();
                    }
                    delayDash = playerModel.dashRecycleDelay;
                });

            playerModel.onDig.AddListener((block, damage) => Debug.Log($"Break : {block.blockID} {damage}"));
        }

        public void Dig(BlockModel block, int damage)
        {
            if (playerModel.digable.Dig(block, damage))
                playerModel.onDig.Invoke(block, damage);
        }

        public void Dash()
        {
            if (playerModel.dashable.Dash(actorModel.body.forward))
                playerModel.onDash.Invoke();
        }
    }
}

