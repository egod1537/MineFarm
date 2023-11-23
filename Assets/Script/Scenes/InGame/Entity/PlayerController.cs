using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Item.Digable;
using Minefarm.Item.Shootable;
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

        protected DefaultDigable defaultDigable;
        public override void Awake()
        {
            base.Awake();

            SubscribeControl();
            SubscribeDash();
            SubscribeKillEntity();

            this.UpdateAsObservable()
                .Where(_ => playerModel.exp >= playerModel.GetNextLevelExp())
                .Subscribe(_ =>
                {
                    long diff = playerModel.GetNextLevelExp();
                    playerModel.level++;
                    playerModel.exp -= diff;
                });

            defaultDigable = new DefaultDigable(actorModel);
        }
        
        private void SubscribeDash()
        {
            this.UpdateAsObservable()
            .Where(_ => InputHandler.GetSubInteraction())
            .Subscribe(_ => Dash());

            float delayDash = 0f;
            this.UpdateAsObservable()
                .Where(_ => playerModel.dashCount < PlayerModel.MAX_DASH_COUNT)
                .Subscribe(_ => delayDash -= Time.deltaTime);
            this.UpdateAsObservable()
                .Where(_ => delayDash <= 0f)
                .Subscribe(_ =>
                {
                    if (playerModel.dashCount < PlayerModel.MAX_DASH_COUNT)
                    {
                        playerModel.dashCount++;
                        playerModel.onDashCharged.Invoke();
                    }
                    delayDash = playerModel.dashRecycleDelay;
                });
        }

        private void SubscribeControl()
        {
            this.UpdateAsObservable()
                .Where(_ => InputHandler.GetJump())
                .Subscribe(_ => base.Jump());

            this.UpdateAsObservable()
                .Select(_ =>
                {
                    float dx = InputHandler.GetAxisX(), dy = InputHandler.GetAxisY();
                    Vector3 ret = new Vector3(dx + dy, 0f, dy - dx);
                    if (ret.sqrMagnitude > 1f) ret.Normalize();
                    return ret;
                })
                .Where(dir => !Mathf.Approximately(dir.sqrMagnitude, 0f))
                .Subscribe(dir => base.Move(dir));

            this.UpdateAsObservable()
                .Where(_ => InputHandler.GetMainInteraction())
                .Subscribe(_ => FowardAction());
        }

        private void SubscribeKillEntity()
        {
            actorModel.onKillEntity.AddListener(target =>
            {
                actorModel.exp += target.exp;
                actorModel.gold += target.gold;
            });
        }

        public override void Shoot(Vector3 direction)
        {
            IShootable shootable = defaultShootable;
            if (playerModel.handleItem != null) 
                shootable = playerModel.handleItem.shootable;

            if (shootable.Shoot(direction))
                actorModel.onShoot.Invoke(direction);
        }
        public override void Dig(BlockModel block, int damage)
        {
            IDigable digable = defaultDigable;
            if (playerModel.handleItem != null)
                digable = playerModel.handleItem.digable;

            if (digable.Dig(block, damage))
                actorModel.onDig.Invoke(block, damage);
        }

        public void Dash()
        {
            if (playerModel.dashable.Dash(actorModel.body.forward))
                playerModel.onDash.Invoke();
        }
    }
}

