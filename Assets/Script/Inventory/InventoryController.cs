using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minefarm.Inventory.InventoryModel;

namespace Minefarm.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        protected InventoryModel model;
        public InventoryController(InventoryModel model)
        {
            this.model = model;
        }
        public bool Add(ItemModel item)
        {
            if (AddItem(item)) return true;
            if (AddQucikItem(item)) return true;
            return false;
        }
        public bool Remove(ItemCategory category, int slot, int count)
        {
            if (RemoveItem(category, slot, count)) return true;
            if (RemoveQuickItem(slot, count)) return true;
            return false;
        }
        public bool Remove(ItemCategory category, int slot)
        {
            if (RemoveItem(category, slot)) return true;
            if (RemoveQuickItem(slot)) return true;
            return false;
        }
        public bool Remove(ItemModel item)
        {
            if (RemoveItem(item)) return true;
            if (RemoveQuickItem(item)) return true;
            return false;
        }

        private bool AddItem(ItemModel item)
        {
            ItemList items = model[item.category];
            int slot = -1;
            for (int i = 0; i < COUNT_ITEM_SLOT; i++)
                if (items[i] != null &&
                    items[i].itemID == item.itemID &&
                    items[i].count + item.count <= items[i].maxCount) slot = i;

            if (slot == -1)
            {
                slot = 0;
                while (slot < COUNT_ITEM_SLOT && items[slot] != null) slot++;

                if (slot == COUNT_ITEM_SLOT) return false;

                item.SetOwner(model.owner);
                model[item.category, slot] = item;
            }
            else model[item.category, slot].count += item.count;

            model.onUpdate.Invoke();

            return true;
        }
        private bool AddQucikItem(ItemModel item)
        {
            int slot = -1;
            for(int i=0; i < COUNT_QUICK_SLOT; i++)
            {
                ItemModel now = model.quick[i];
                if (now != null &&
                    now.itemID == item.itemID &&
                    now.count + item.count <= now.maxCount) slot = i;
            }

            if (slot == -1)
            {
                slot = 0;
                while (slot < COUNT_QUICK_SLOT && model.quick[slot] != null)
                    slot++;

                if (slot == COUNT_QUICK_SLOT) return false;

                item.SetOwner(model.owner);
                model.quick[slot] = item;
            }
            else model.quick[slot].count += item.count;

            model.onUpdate.Invoke();
            return true;
        }

        private bool RemoveItem(ItemCategory category, int slot, int count)
        {
            ItemList items = model[category];
            if (items[slot] == null) return false;

            ItemModel selected = items[slot];
            selected.count -= count;
            if (selected.count <= 0) model[category, slot] = null;
            model.onUpdate.Invoke();

            return true;
        }
        private bool RemoveItem(ItemCategory category, int slot)
        {
            ItemList items = model[category];
            if (items[slot] == null) return false;
            return RemoveItem(category, slot, items[slot].count);
        }
        private bool RemoveItem(ItemModel item)
        {
            if (item == null) return false;
            ItemList now = model[item.category];
            for (int i = 0; i < COUNT_ITEM_SLOT; i++)
                if (now[i] == item) return RemoveItem(item.category, i);
            return true;
        }
        private bool RemoveQuickItem(int slot, int count)
        {
            if (model.quick[slot] == null) return false;

            ItemModel selected = model.quick[slot];
            selected.count -= count;
            if (selected.count <= 0) model.quick[slot] = null;
            model.onUpdate.Invoke();

            return true;
        }
        private bool RemoveQuickItem(int slot)
        {
            if (model.quick[slot] == null) return false;
            return RemoveQuickItem(slot, model.quick[slot].count);
        }
        private bool RemoveQuickItem(ItemModel item)
        {
            if (item == null) return false;
            for (int i = 0; i < COUNT_QUICK_SLOT; i++)
                if (model.quick[i] == item) return RemoveQuickItem(i);
            return true;
        }

        public bool Move(ItemCategory category, int from, int to)
        {
            //이동하려는 대상이 존재하지 않는 경우
            if (model[category, from] == null) return false;

            //to로 이동할 수 있는 경우
            if(model[category, to] == null)
            {
                model[category, to] = model[category, from];
                model[category, from] = null;
            }
            //to로 이동할 수 없는 경우
            else
            {
                //1. from과 to가 같은 아이템 타입인 경우
                if (model[category, to].itemID == model[category, from].itemID)
                {
                    int count = Mathf.Min(
                        model[category, to].remainCount, 
                        model[category, from].count);

                    model[category, to].count += count;
                    model[category, from].count -= count;

                    if (model[category, from].count <= 0)
                        model[category, from] = null;
                }
                //2. 다른 아이템 타입인 경우
                else return false;
            }

            model.onUpdate.Invoke();

            return true;
        }
    }
}

