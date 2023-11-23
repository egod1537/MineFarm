using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minefarm.Inventory.UI.Stat
{
    public class ViewPlayerProfile : InventoryUIHahaviour
    {
        public TextMeshProUGUI lblLevel;
        public Image imgHp;
        public TextMeshProUGUI lblHp;
        public Image imgExp;
        public TextMeshProUGUI lblExp;

        private void Start()
        {
            inventory.onUpdate.AddListener(() => DisplayPlayerProfile());
            presenter.onOpenInventory.AddListener(() => DisplayPlayerProfile());
            presenter.onUpdateStat.AddListener(() => DisplayPlayerProfile());
            DisplayPlayerProfile();
        }

        public void DisplayPlayerProfile()
        {
            lblLevel.text = $"{player.level}";

            float hpRatio = 1.0f * player.hp / player.stats.calculatedMaxHp;
            imgHp.DOKill();
            imgHp.DOFillAmount(hpRatio, 0.25f);
            imgHp.DOColor(Color.gray, 0.125f)
                .OnComplete(() => imgHp.DOColor(Color.white, 0.125f));
            lblHp.text = $"{(hpRatio * 100f).ToString("00.0")}%";

            float expRatio = 1.0f * player.exp / player.GetNextLevelExp();
            imgExp.DOKill();
            imgExp.DOFillAmount(expRatio, 0.25f);
            imgExp.DOColor(Color.gray, 0.125f)
                .OnComplete(() => imgExp.DOColor(Color.white, 0.125f));
            lblExp.text = $"{(expRatio * 100f).ToString("00.0")}%";
        }
    }
}