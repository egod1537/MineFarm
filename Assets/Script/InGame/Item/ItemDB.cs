using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item
{
    public static class ItemDB
    {
        public static EquipmentType GetEquipmentType(ItemID itemID)
        {
            return EquipmentType.MainWeapon;
        }
    }
}

