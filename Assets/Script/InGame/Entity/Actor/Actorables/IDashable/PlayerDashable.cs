using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Entity.Actor.Player;

namespace Minefarm.Entity.Actor.Dashable
{
    public class PlayerDashable : Actorable, IDashable
    {
        private float delayDash;
        private PlayerModel playerModel { get => base.actor as PlayerModel; }

        public PlayerDashable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => delayDash -= Time.deltaTime);
        }

        public bool CanDash(Vector3 direction)
        {
            return delayDash <= 0f && playerModel.dashCount > 0;
        }

        public bool Dash(Vector3 direction)
        {
            if (!CanDash(direction)) return false;
            float power = playerModel.calculatedSpeed * IDashable.CONSTANT_DASH_FORCE;
            rigidbody.AddForce(direction * power, ForceMode.Impulse);
            actor.Invoke(() => {
                rigidbody.AddForce(-direction * power/2f, ForceMode.Impulse);
                playerModel.onDashEnd.Invoke();
            }, 0.25f);
            delayDash = IDashable.DELAY_DASH;
            playerModel.dashCount--;
            return true;
        }
    }
}