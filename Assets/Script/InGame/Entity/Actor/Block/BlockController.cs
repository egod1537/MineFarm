using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity.Actor.Block
{
    public class BlockController : ActorController
    {
        const float DELAY_RECOVERY = 1.25f;

        protected BlockModel blockModel { get => actorModel as BlockModel; }

        float delayRecovery;
        private void Awake()
        {
            base.Awake();

            actorModel.onKill.AddListener(() => Destroy(gameObject));

            actorModel.onDamage.AddListener((other, damage, isCritical) => 
                delayRecovery = DELAY_RECOVERY);
            this.UpdateAsObservable()
                .Where(_ => actorModel.hp < actorModel.calculatedMaxHp)
                .Subscribe(_ => delayRecovery -= Time.deltaTime);
            this.UpdateAsObservable()
                .Where(_ => delayRecovery <= 0f)
                .Subscribe(_ =>
                {
                    actorModel.hp = actorModel.calculatedMaxHp;
                    blockModel.onRecovery.Invoke();
                    delayRecovery = DELAY_RECOVERY;
                });
        }
    }
}

