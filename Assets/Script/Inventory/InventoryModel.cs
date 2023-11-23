using Minefarm.Entity.Actor;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Inventory
{
    /// <summary>
    /// 0 ~ 29 : Item Slot
    /// 30 ~ 36 : Quick Slot
    /// 37 ~ 43 : Equipment Slot
    /// </summary>
    public class InventoryModel
    {
        [Serializable]
        public class ItemList : List<ItemModel>
        {
            public ItemList() : base() { }
            public ItemList(IEnumerable<ItemModel> collection) : base(collection)
            {
            }
        }
        [Serializable]
        public class InventoryDictionary : SerializableDictionary<ItemCategory, ItemList> { }

        public const int COUNT_ITEM_SLOT = 30;
        public const int COUNT_QUICK_SLOT = 7;
        public const int COUNT_EQUIPMENT_SLOT = 6;

        public const int INDEX_ITEM_SLOT = COUNT_ITEM_SLOT;
        public const int INDEX_QUICK_SLOT = INDEX_ITEM_SLOT + COUNT_QUICK_SLOT;
        public const int INDEX_EQUIPMENT_SLOT = INDEX_QUICK_SLOT + COUNT_EQUIPMENT_SLOT;

        public const int MAX_SLOT = COUNT_ITEM_SLOT + COUNT_QUICK_SLOT + COUNT_EQUIPMENT_SLOT;

        public UnityEvent onUpdate = new();     

        public ActorModel owner;

        public InventoryDictionary item;
        [SerializeField]
        public ItemList quick;
        [SerializeField]
        public EquipmentItemFrame equipment;

        public InventoryController controller;

        public ItemList this[ItemCategory category]
        {
            get => GetList(category);
        }
        public ItemModel this[ItemCategory category, int slot]
        {
            get => Get(category, slot);
            set => Set(category, slot, value);
        }

        public InventoryModel(ActorModel owner)
        {
            this.owner = owner;
            this.controller = new InventoryController(this);

            item = new();
            foreach (ItemCategory type in Enum.GetValues(typeof(ItemCategory)))
                item.Add(type, new ItemList(new ItemModel[COUNT_ITEM_SLOT]));

            quick = new ItemList(new ItemModel[COUNT_QUICK_SLOT]);
            equipment = new EquipmentItemFrame(this);
        }

        public ItemList GetList(ItemCategory category)
        {
            ItemList ret = new();
            for (int i = 0; i < MAX_SLOT; i++) ret.Add(Get(category, i));
            return ret;
        }

        public ItemModel Get(ItemCategory category, int slot)
        {
            if (slot < 0 || slot >= MAX_SLOT) return null;
            return slot switch
            {
                < INDEX_ITEM_SLOT => item[category][slot],
                < INDEX_QUICK_SLOT => quick[GetSlot2QuickSlot(slot)],
                < INDEX_EQUIPMENT_SLOT => equipment[GetSlot2EquipmentType(slot)],
                _ => null
            };
        }
        public ItemModel Get(int slot) => Get(ItemCategory.Equipment, slot);
        public void Set(ItemCategory category, int slot, ItemModel other)
        {
            if (slot < 0 || slot >= MAX_SLOT) return;
            if (slot < INDEX_ITEM_SLOT)
                item[category][slot] = other;
            else if (slot < INDEX_QUICK_SLOT)
                quick[GetSlot2QuickSlot(slot)] = other;
            else if (slot < INDEX_EQUIPMENT_SLOT && other is EquipmentItemModel)
                equipment[GetSlot2EquipmentType(slot)] = other as EquipmentItemModel;

            onUpdate.Invoke();
        }
        public void Set(int slot, ItemModel other) 
            => Set(ItemCategory.Equipment, slot, other);

        public EquipmentItemType GetSlot2EquipmentType(int slot)
            => (EquipmentItemType)(slot - INDEX_QUICK_SLOT + 1);
        public int GetEquipmentType2Slot(EquipmentItemType slot)
            => (int)slot + INDEX_QUICK_SLOT - 1;
        public int GetSlot2QuickSlot(int slot)
            => slot - INDEX_ITEM_SLOT;
        public int GetQuickSlot2Slot(int slot)
            => slot + INDEX_ITEM_SLOT;

        public int FindSlot(ItemCategory category, ItemModel model)
        {
            for (int i = 0; i < MAX_SLOT; i++)
                if (Get(category, i) == model) return i;
            return -1;
        }
    }
}