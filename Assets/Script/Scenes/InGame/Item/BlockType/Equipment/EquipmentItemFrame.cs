using GoogleSheet.Protocol.v2.Req;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Player;
using Minefarm.Inventory;
using Minefarm.Inventory.UI.Slot;
using Minefarm.Item.Equipment;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Item.Equipment
{
    [Serializable]
    public class EquipmentItemFrame
    {
        public UnityEvent<EquipmentItemType> onEquip = new();
        public UnityEvent<EquipmentItemType> onUnEquip = new();

        private Dictionary<EquipmentItemType, EquipmentItemModel> equip;

        private InventoryModel inventory;

        public EquipmentItemFrame(InventoryModel inventory)
        {
            this.inventory = inventory;

            equip = new();
            foreach (EquipmentItemType type in Enum.GetValues(typeof(EquipmentItemType)))
                equip.Add(type, null);
        }

        public bool Equip(EquipmentItemModel model)
        {
            if (equip[model.type] != null) return false;

            model.isEquip = true;
            equip[model.type] = model;

            inventory.Set(inventory.GetEquipmentType2Slot(model.type), model);
            StatInjector.InjectItem(inventory.owner, model);
            onEquip.Invoke(model.type);
            return true;
        }
        public bool UnEquip(EquipmentItemType type)
        {
            if (equip[type] == null) return false;
            if (!inventory.controller.Add(equip[type])) return false;

            inventory.Set(inventory.GetEquipmentType2Slot(type), null);
            StatInjector.InjectItem(inventory.owner, equip[type]);

            equip[type].isEquip = false;
            equip[type] = null;

            onUnEquip.Invoke(type);
            return true;
        }

        public EquipmentItemModel this[EquipmentItemType type]
        {
            get => equip[type];
            set => Equip(value);
        }
    }
}