using Minefarm.Effect;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity.Actor.Block
{
    public class BlockView : ActorView
    {
        const float DELAY_INVISIBLE = 0.125f;

        protected BlockModel blockModel { get => actorModel as BlockModel; }
        protected Transform blockBody { get => actorModel.body; }
        private MeshRenderer _meshRenderer;
        protected MeshRenderer meshRenderer {
            get => _meshRenderer ??= blockBody.GetComponent<MeshRenderer>();
        }

        public int invisibleLevel = 0;
        float delayInvisible;

        public void Awake()
        {
            base.Awake();

            SetInvisibleLevel(invisibleLevel);

            this.UpdateAsObservable()
                .Where(_ => invisibleLevel > 0 && delayInvisible <= 0f)
                .Subscribe(_ => SetInvisibleLevel(--invisibleLevel));

            this.UpdateAsObservable()
                .Subscribe(_ => delayInvisible -= Time.deltaTime);

            blockModel.onDamage.AddListener((other, damage, isCritical)
                => Effector.CrackBlock(blockModel));

            blockModel.visible.Subscribe(v => blockModel.body.gameObject.SetActive(v));
        }

        public void SetInvisibleLevel(int level)
        {
            meshRenderer.material = BlockDB.LoadInvisibleMaterial(level);
            invisibleLevel = level;
            delayInvisible = DELAY_INVISIBLE;
        }
        public void SetInvisible(bool flag) => SetInvisibleLevel((flag?5:0));
    }
}