using Minefarm.Inventory.UI.Slot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minefarm.Inventory.InventoryModel;

namespace Minefarm.InGame.UI.Bottom
{
    public class ViewQuickSlotItems : MineBehaviour
    {
        const string PATH_SLOT_TEMPLATE = "UI/Inventory/Slot Template";

        List<Transform> slots;

        GameObject slotTemplate;
        private void Awake()
        {
            slotTemplate = Resources.Load(PATH_SLOT_TEMPLATE) as GameObject;

            InitializeSlots();
        }
        private void Start()
        {
            player.inventory.onUpdate.AddListener(() => UpdateQuickSlotItem());

            UpdateQuickSlotItem();
        }

        private void InitializeSlots()
        {
            int sz = transform.childCount;
            slots = new();
            for (int i = 0; i < sz; i++)
                slots.Add(transform.GetChild(i));
        }

        private void RemoveQuickSlotItem()
        {
            int sz = slots.Count;
            for (int i = 0; i < sz; i++)
                slots[i].DestroyChild();
        }

        private void UpdateQuickSlotItem()
        {
            RemoveQuickSlotItem();

            int sz = slots.Count;
            ItemList items = player.inventory.quick;
            for (int i=0; i < sz; i++)
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