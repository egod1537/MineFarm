using Minefarm.Entity.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Item
{
    public static class ItemDB
    {
        const string DB_PATH = "Entity/Item";

        private static Dictionary<ItemID, GameObject> db_item = new();
        public static GameObject GetItemModel(ItemID itemID)
        {
            if (!db_item.ContainsKey(itemID))
                db_item.Add(itemID, Resources.Load($"{DB_PATH}/{itemID}") as GameObject);
            return db_item[itemID];
        }
        public static EquipmentType GetEquipmentType(ItemID itemID)
        {
            return EquipmentType.MainWeapon;
        }
    }
}

