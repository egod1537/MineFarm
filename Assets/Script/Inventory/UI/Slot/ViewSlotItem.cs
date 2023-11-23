using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minefarm.Inventory.UI.Slot
{
    public class ViewSlotItem : MonoBehaviour
    {
        private ItemModel _item;
        public ItemModel item
        {
            get => _item;
            set => SetItem(value);
        }

        public Image imgIcon;
        public TextMeshProUGUI lblCount;

        private void SetItem(ItemModel itemModel)
        {
            imgIcon.sprite = ItemDB.GetItemIcon(itemModel.itemID);

            lblCount.enabled = itemModel.count != 1;
            lblCount.text = itemModel.count.ToString();
            _item = itemModel;
        }
    }
}