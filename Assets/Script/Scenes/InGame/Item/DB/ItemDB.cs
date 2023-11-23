using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UGS;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Item
{
    public static class ItemDB
    {
        const string PATH_ITEM_MODEL = "Item";
        const string PATH_DROP_ITEM_MODEL = "Entity/DropItem";
        const string PATH_ITEM_ICON = "UI/Item/Icon";

        private static Dictionary<ItemID, GameObject> db_item_model = new();
        private static Dictionary<ItemID, Sprite> db_item_icon = new();
        private static GameObject drop_item_model;

        static ItemDB()
        {
            UnityGoogleSheet.Load<ItemSheet.Stat>();
            UnityGoogleSheet.Load<ItemSheet.Attribute>();
        }

        public static GameObject GetItemModel(ItemID itemID)
        {
            if (!db_item_model.ContainsKey(itemID))
                db_item_model.Add(itemID, Resources.Load($"{PATH_ITEM_MODEL}/{itemID}") as GameObject);
            if (db_item_model[itemID] == null)
                db_item_model[itemID] = Resources.Load($"{PATH_ITEM_MODEL}/{itemID}") as GameObject;
            return db_item_model[itemID];
        }
        public static GameObject GetDropItemModel()
        {
            if (drop_item_model == null)
                drop_item_model = Resources.Load(PATH_DROP_ITEM_MODEL) as GameObject;
            return drop_item_model;
        }
        public static Sprite GetItemIcon(ItemID itemID)
        {
            if (!db_item_icon.ContainsKey(itemID))
                db_item_icon.Add(itemID, Resources.Load<Sprite>($"{PATH_ITEM_ICON}/{itemID}"));
            if (db_item_icon[itemID] == null)
                db_item_icon[itemID] = Resources.Load<Sprite>($"{PATH_ITEM_ICON}/{itemID}");
            return db_item_icon[itemID];
        }
        public static ItemCategory GetCategory(ItemID itemID)
            => ItemSheet.Stat.StatMap[itemID].category;
        public static EquipmentItemType GetEquipmentType(ItemID itemID)
            => ItemSheet.Attribute.AttributeMap[itemID].equipmentType;
    }
}

