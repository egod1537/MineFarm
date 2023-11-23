using Minefarm.Entity;
using Minefarm.Entity.Actor.Player;
using Minefarm.Entity.DropItem;
using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Minefarm.Item.Actionable
{
    public class DropItemAction : IItemActionable
    {
        public void Action(ItemModel item, PlayerModel player)
        {
            Spawner.DropItem(player.centerPosition, item);

            if(item is EquipmentItemModel)
            {
                EquipmentItemModel model = item as EquipmentItemModel;
                if (model.isEquip) player.inventory.equipment.UnEquip(model.type);
            }
                
            player.inventory.controller.Remove(item);
        }

        public void DrawButton(ItemModel item, TextMeshProUGUI label)
        {
            label.text = "¹ö¸®±â";
        }
    }
}