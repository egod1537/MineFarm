using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Minefarm.Inventory.UI;

namespace Minefarm.InGame.UI
{
    public class ViewPlayerHp : MineBehaviour
    {
        public Transform hpBar;
        public TextMeshProUGUI txtHp;

        Image imgHp;

        private void Awake()
        {
            imgHp = hpBar.GetComponent<Image>();
            player.onHp.AddListener(hp => UpdateHp(hp));
        }
        private void Start()
        {
            player.onSpawn.AddListener(() => UpdateHp(player.hp));
            player.inventory.equipment.onEquip.AddListener(type => UpdateHp(player.hp));
            player.inventory.equipment.onUnEquip.AddListener(type => UpdateHp(player.hp));
        }
        public void UpdateHp(int hp)
        {
            int maxHp = player.stats.calculatedMaxHp;
            txtHp.text = $"{hp.ToString("#,##0")} / {maxHp.ToString("#,##0")}";

            float ratio = Mathf.Max(0f, 1.0f * hp / maxHp);

            imgHp.DOKill();
            imgHp.DOFillAmount(ratio, 0.25f);
            imgHp.DOColor(Color.gray, 0.125f)
                .OnComplete(() => imgHp.DOColor(Color.white, 0.125f));
        }
    }
}

