using Minefarm.Entity.Actor;
using Minefarm.Item.Block;
using Minefarm.Item.Consumption;
using Minefarm.Item.Equipment;
using Minefarm.Item.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

namespace Minefarm.Item
{
    public static class Itemer
    {
        static Itemer()
        {
            UnityGoogleSheet.Load<ItemSheet.Stat>();
            UnityGoogleSheet.Load<ItemSheet.Attribute>();
        }

        public static ItemModel CreateItemModel(ItemID itemID, ActorModel owner)
        {
            var data = ItemSheet.Stat.StatMap[itemID];

            ItemModel item = data.category switch{
                ItemCategory.Equipment => new EquipmentItemModel(owner),
                ItemCategory.Consumption => new ConsumptionItemModel(owner),
                ItemCategory.Block => new BlockItemModel(owner),
                ItemCategory.Other => new OtherItemModel(owner),
            };

            item.itemID = itemID;
            item.category = data.category;

            item.stat.maxHp = data.maxHp.value;
            item.stat.maxHpPercent = data.maxHpPercent.value;

            item.stat.attack = data.attack.value;
            item.stat.attackPercent = data.attackPercent.value;

            item.stat.attackSpeed = data.attackSpeed.value;
            item.stat.attackSpeedPercent = data.attackSpeedPercent.value;

            item.stat.attackRange = data.attackRange.value;
            item.stat.attackRangePercent = data.attackRangePercent.value;

            item.stat.bulletModel = data.bulletModel;

            item.stat.defense = data.defense.value;
            item.stat.defensePercent = data.defensePercent.value;
            item.stat.durabilityNegation = data.durabilityNegation.value;

            item.stat.criticalChance = data.criticalChance.value;
            item.stat.criticalDamage = data.criticalDamage.value;

            item.stat.speed = data.speed.value;
            item.stat.speedPercent = data.speedPercent.value;

            item.stat.jumpPower = data.jumpPower.value;
            return item;
        }

        //private static ItemID ConvertItemID(ItemID itemID)
        //    => Enum.Parse(ItemStatDB.Data.DataMap[$"{itemID}"].);
    }
}

