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
        const float DELAY_REVEAL = 5.0f*DELAY_INVISIBLE;

        protected BlockModel blockModel { get => actorModel as BlockModel; }
        protected Transform blockBody { get => actorModel.body; }
        private MeshRenderer _meshRenderer;
        protected MeshRenderer meshRenderer {
            get => _meshRenderer ??= blockBody.GetComponent<MeshRenderer>();
        }

        public int invisibleLevel = 0;
        float delayInvisible;
        float delayReveal;

        public void Awake()
        {
            base.Awake();

            SetInvisibleLevel(invisibleLevel);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    delayReveal -= Time.deltaTime;
                    delayInvisible -= Time.deltaTime;
                });

            this.UpdateAsObservable()
                .Where(_ => invisibleLevel > 0 && delayInvisible <= 0f)
                .Subscribe(_ => SetInvisibleLevel(--invisibleLevel));
            this.UpdateAsObservable()
                .Where(_ => delayReveal <= 0.0f)
                .Where(_ => blockModel.reveal.Value)
                .Subscribe(_ => blockModel.reveal.Value = false);

            blockModel.onDamage.AddListener((other, damage, isCritical)
                => Effector.CrackBlock(blockModel));

            InitializeAboutVisible();
        }

        private void InitializeAboutVisible()
        {
            blockModel.visible.Subscribe(v => UpdateVisible());
            blockModel.reveal.Subscribe(v => UpdateVisible());
        }

        public void SetInvisibleLevel(int level)
        {
            meshRenderer.material = BlockDB.LoadInvisibleMaterial(level);
            invisibleLevel = level;
            delayInvisible = DELAY_INVISIBLE;
        }
        public void SetInvisible(bool flag) => SetInvisibleLevel((flag?5:0));

        public void SetReveal()
        {
            blockModel.reveal.Value = true;
            delayReveal = DELAY_REVEAL;
        }
        public void UpdateVisible()
            => blockModel.body.gameObject.SetActive(
                blockModel.reveal.Value || blockModel.visible.Value);
    }
}