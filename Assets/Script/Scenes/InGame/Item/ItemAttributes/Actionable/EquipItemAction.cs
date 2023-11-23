using Minefarm.Entity.Actor.Player;
using Minefarm.Inventory;
using Minefarm.Item.Equipment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
namespace Minefarm.Item.Actionable
{
    public class EquipItemAction : IItemActionable
    {
        public void Action(ItemModel item, PlayerModel player)
        {
            EquipmentItemModel model = item as EquipmentItemModel;

            if (!model.isEquip) Equip(model, player);
            else UnEquip(model, player);
        }

        public void DrawButton(ItemModel item, TextMeshProUGUI label)
        {
            EquipmentItemModel model = item as EquipmentItemModel;

            label.text = model.isEquip ? "¿Â¬¯ «ÿ¡¶" : "¿Â¬¯";
        }

        private void Equip(EquipmentItemModel model, PlayerModel player)
        {
            InventoryModel inventory = player.inventory;
            EquipmentItemFrame equip = inventory.equipment;

            if (equip[model.type] != null && !equip.UnEquip(model.type))
                return;

            int slot = inventory.FindSlot(model.category, model);
            if (slot == -1) return;
            inventory.controller.Move(
                model.category, 
                slot, 
                inventory.GetEquipmentType2Slot(model.type));
            equip.Equip(model);
        }

        private void UnEquip(EquipmentItemModel model, PlayerModel player)
        {
            InventoryModel inventory = player.inventory;
            EquipmentItemFrame equip = inventory.equipment;

            equip.UnEquip(model.type);
        }
    }
}