using Minefarm.Item;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Inventory.UI.Slot
{
    public class UIInventoryCategory : InventoryUIHahaviour
    {
        List<ColorGroup> colorGroups;
        private void Awake()
        {
            colorGroups = new List<ColorGroup>();
            int sz = transform.childCount;
            for (int i = 0; i < sz; i++)
                colorGroups.Add(transform.GetChild(i).GetComponent<ColorGroup>());

            ChangeCategory((int)selectedCategory);
        }

        public void ChangeCategory(int category)
        {
            foreach (var col in colorGroups)
                col.color = Color.gray;

            colorGroups[category].color = Color.white;
            selectedCategory = (ItemCategory)category;

            presenter.onChangeCategory.Invoke(selectedCategory);
        }
    }
}