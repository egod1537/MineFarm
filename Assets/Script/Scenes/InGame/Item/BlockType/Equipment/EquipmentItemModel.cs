using Minefarm.Entity.Actor;
using Minefarm.Item.Actionable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item.Equipment
{
    public class EquipmentItemModel : ItemModel
    {
        public bool isEquip;
        public EquipmentItemType type { get => ItemDB.GetEquipmentType(itemID); }

        public EquipmentItemModel(ActorModel owner) : base(owner)
        {
            maxCount = 1;
            controller.actionable.Add(new EquipItemAction());
        }
    }
}
