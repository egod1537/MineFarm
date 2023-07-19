using Minefarm.Entity.Actor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
namespace Minefarm.Item.Equipment
{
    public class EquipmentFrame
    {
        public Subject<EquipmentType> onEquip = new();
        public Subject<EquipmentType> onUnEquip = new();

        private Dictionary<EquipmentType, EquipmentModel> equip;

        private PlayerModel player;

        public EquipmentFrame(PlayerModel player)
        {
            this.player = player;

            equip = new();
            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
                equip.Add(type, null);
        }

        public bool Equip(EquipmentType type, EquipmentModel model)
        {
            if (equip[type] != null) return false;
            equip[type] = model;
            onEquip.OnNext(type);
            return true;
        }
        public bool UnEquip(EquipmentType type, EquipmentModel model)
        {
            if (equip[type] == null) return false;
            equip[type] = null;
            onUnEquip.OnNext(type);
            return true;
        }

        public EquipmentModel this[EquipmentType type]
        {
            get => equip[type];
        }
    }
}