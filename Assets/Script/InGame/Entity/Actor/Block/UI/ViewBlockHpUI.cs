using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UniRx.Triggers;
using UniRx;
using Minefarm.Entity.Actor.Block;

namespace Minefarm.Entity.Actor.Monster.UI
{
    public class ViewBlockHpUI : MonoBehaviour
    {
        const float TIME_VIEW_UI = 3f;

        public Transform blcokBar;
        BlockModel model;
        SpriteRenderer spriteRenderer;

        float timeViewUI;

        private void Awake()
        {
            model = GetComponentInParent<BlockModel>();
            spriteRenderer = blcokBar.GetComponent<SpriteRenderer>();

            model.onSpawn.AddListener(() => UpdateBar());
            model.onDamage.AddListener((target, damage, isCritical) => UpdateBar());

            this.UpdateAsObservable()
                .Subscribe(_ => timeViewUI -= Time.deltaTime);

            this.UpdateAsObservable()
                .Where(_ => timeViewUI <= 0f)
                .Subscribe(_ => transform.SetActiveChild(false));
        }
        private void Start()
        {
            transform.SetActiveChild(false);
        }
        public void UpdateBar()
        {
            blcokBar.DOScaleX(Mathf.Max(0f, 1.0f * model.hp / model.calculatedMaxHp), 0.25f);
            spriteRenderer.DOColor(Color.gray, 0.125f)
                .OnComplete(() => spriteRenderer.DOColor(Color.white, 0.125f));

            transform.SetActiveChild(true);
            timeViewUI = TIME_VIEW_UI;
        }
    }
}