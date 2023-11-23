using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Item
{
    [Serializable]
    public class ItemResourceTable
    {
        public ItemID itemID;
        public ItemCategory category { get => ItemDB.GetCategory(itemID); }
        
        public bool isStat { get => ItemSheet.Stat.GetDictionary().ContainsKey(itemID); }
        public bool isAttribute { get => ItemSheet.Attribute.GetDictionary().ContainsKey(itemID); }
        public bool isItemModel { get => ItemDB.GetItemModel(itemID) != null; }
        public bool isItemIcon { get => ItemDB.GetItemIcon(itemID) != null; }

        public ItemResourceTable(ItemID itemID)
        {
            this.itemID = itemID;
        }

        public bool IsCorrect()
            => isStat &&
            isItemModel &&
            isItemIcon;
    }

    [CreateAssetMenu(
        fileName ="Item Resource Validation Table", 
        menuName ="Scriptable Object/Table/Item Resource Validation Table", 
        order=int.MaxValue)]
    public class ItemResourceValidationTable : ScriptableObject
    {
        [Serializable]
        public class ItemResourceTableDictionary : SerializableDictionary<ItemID, ItemResourceTable> { }
        [SerializeField]
        public ItemResourceTableDictionary table = new();

        public void Validate()
        {
            table = new();
            foreach(ItemID id in Enum.GetValues(typeof(ItemID)))
                table.Add(id, new ItemResourceTable(id));
        }
    }
}