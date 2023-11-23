using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Minefarm.Inventory.UI.Slot
{
    public class ViewPlayerGold : InventoryUIHahaviour
    {
        TextMeshProUGUI lblGold;

        private void Awake()
        {
            lblGold = GetComponent<TextMeshProUGUI>();

            presenter.onOpenInventory.AddListener(() => UpdateText());
            presenter.onUpdateInventory.AddListener(() => UpdateText());
        }

        public void UpdateText()
        {
            lblGold.text = player.gold.ToString("#,##0");
        }
    }
}