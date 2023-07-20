using Minefarm.Entity.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Item.Equipment
{
    public class EquipmentModel : ItemModel
    {
        public EquipmentType type { get => ItemDB.GetEquipmentType(itemID); }
    }
}
