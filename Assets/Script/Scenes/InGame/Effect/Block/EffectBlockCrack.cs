using Minefarm.Effect;
using Minefarm.Entity.Actor.Block;
using UniRx;
using UnityEngine;
namespace Minefarm.InGame.Effect.Block
{
    public class EffectBlockCrack : MonoBehaviour
    {
        public BlockModel blockModel;
        public ReactiveProperty<int> level;

        Material material;
        private void Awake()
        {
            material = GetComponent<MeshRenderer>().material;

            level.Subscribe(_ => material.SetTexture("_Albedo",
                EffectDB.LoadCrackTexture(level.Value)));
        }
        public void SubscribeBlock(BlockModel block)
        {
            blockModel = block;
            blockModel.onRecovery.AddListener(() => Destroy(this.gameObject));
            blockModel.onDead.AddListener(() => Destroy(this.gameObject));
            blockModel.onDamage.AddListener((other, damage, isCritical) =>
            {
                float ratio = 1.0f * blockModel.hp / blockModel.stats.calculatedMaxHp;
                level.Value = Mathf.FloorToInt(10f * ratio);
            });
        }
    }
}

