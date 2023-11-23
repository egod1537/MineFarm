using Minefarm.Entity.Actor;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Minefarm.Inventory.UI.Stat
{
    public class ViewPlayerStat : InventoryUIHahaviour
    {
        public TextMeshProUGUI lblMaxHp;
        public TextMeshProUGUI lblDefense;
        public TextMeshProUGUI lblAttack;
        public TextMeshProUGUI lblAttackSpeed;
        public TextMeshProUGUI lblCritical;
        public TextMeshProUGUI lblSpeed;

        private void Start()
        {
            inventory.onUpdate.AddListener(() => DisplayPlayerStat());
            presenter.onOpenInventory.AddListener(() => DisplayPlayerStat());
            presenter.onUpdateStat.AddListener(() => DisplayPlayerStat());
            DisplayPlayerStat();
        }

        private void DisplayPlayerStat()
        {
            Stats stat = player.stats;
            lblMaxHp.text = stat.calculatedMaxHp.ToString("#,##0");
            lblDefense.text = $"{stat.calculatedDefense.ToString("#,##0")} " +
                $"({stat.calculatedDurabilityNegation.ToString("0.00")}%)";
            lblAttack.text = stat.calculatedAttack.ToString("#,##0");
            lblAttackSpeed.text = $"{stat.calculatedAttackSpeed.ToString("0.00")}/s";
            lblCritical.text = $"{stat.calculatedCriticalChance.ToString("0.00")}% ({(int)(stat.calculatedCriticalDamage * 100f)}%)";
            lblSpeed.text = $"{stat.calculatedSpeed.ToString("0.00")}/s";
        }
    }
}