using Minefarm.InGame;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minefarm.Inventory.InventoryModel;

namespace Minefarm.Inventory.UI.Slot
{
    public class ViewInventorySlotItems : InventoryUIHahaviour
    {
        const string PATH_SLOT_TEMPLATE = "UI/Inventory/Slot Template";

        GameObject slotTemplate;

        private void Awake()
        {
            slotTemplate = Resources.Load(PATH_SLOT_TEMPLATE) as GameObject;

            presenter.onUpdateInventory.AddListener(() => UpdateSlotItems());
        }
        private void RemoveAllSlotItmes()
        {
            int sz = MAX_SLOT;
            for (int i = 0; i < sz; i++)
                transform.GetChild(i).DestroyChild();
        }
        public void UpdateSlotItems()
        {
            RemoveAllSlotItmes();

            ItemList items = player.inventory[selectedCategory];
            int sz = MAX_SLOT;
            for(int i=0; i < sz; i++)
            {
                if (items[i] == null) continue;
                GameObject go = Instantiate(slotTemplate);

                Transform tr = go.transform;
                tr.SetParent(transform.GetChild(i));
                tr.localPosition = Vector3.zero;

                ViewSlotItem view = go.GetComponent<ViewSlotItem>();
                view.item = items[i];
            }
        }
    }
}
