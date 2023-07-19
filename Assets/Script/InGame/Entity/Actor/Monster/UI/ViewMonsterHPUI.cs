using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace Minefarm.Entity.Actor.Monster.UI
{
    public class ViewMonsterHPUI : MonoBehaviour
    {
        public Transform hpBar;
        MonsterModel model;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            model = GetComponentInParent<MonsterModel>();
            spriteRenderer = hpBar.GetComponent<SpriteRenderer>();

            model.onSpawn.AddListener(() => UpdateBar());
            model.onDamage.AddListener((target, damage, isCritical) => UpdateBar());
        }
        public void UpdateBar()
        {
            hpBar.DOScaleX(1.0f * model.hp / model.calculatedMaxHp, 0.25f);
            spriteRenderer.DOColor(Color.white * 0.5f, 0.125f)
                .OnComplete(() => spriteRenderer.DOColor(Color.white, 0.125f));
        }
    }
}