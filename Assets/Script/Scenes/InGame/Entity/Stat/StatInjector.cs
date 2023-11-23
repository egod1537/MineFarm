using GoogleSheet.Protocol.v2.Req;
using Minefarm.Entity.Actor.Block;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Minefarm.Entity.Actor
{
    public static class StatInjector
    {
        public static void InjectMonster(ActorModel actor)
        {
            MonsterSheet.Stat statSheet = 
                MonsterSheet.Stat.GetDictionary()[actor.entityID];

            Stats stats = actor.stats;

            actor.level = statSheet.level;
            actor.exp = statSheet.exp;
            actor.gold = statSheet.gold;

            stats.maxHp = statSheet.maxHp;
            stats.maxHpPercent = statSheet.maxHpPercent;
            actor.hp = stats.calculatedMaxHp;

            stats.attack = statSheet.attack;
            stats.attackPercent = statSheet.attackPercent;

            stats.attackSpeed = statSheet.attackSpeed;
            stats.attackSpeedPercent = statSheet.attackSpeedPercent;

            stats.attackRange = statSheet.attackRange;
            stats.attackRangePercent = statSheet.attackRangePercent;
            stats.bulletModel = statSheet.bulletModel;

            stats.defense = statSheet.defense;
            stats.defensePercent = statSheet.defensePercent;

            stats.defenseRatio = statSheet.defenseRatio;
            stats.durabilityNegation = statSheet.durabilityNegation;

            stats.criticalChance = statSheet.criticalChance;
            stats.criticalDamage = statSheet.criticalDamage;

            stats.speed = statSheet.speed;
            stats.speedPercent = statSheet.speedPercent;

            stats.jumpPower = statSheet.jumpPower;
        }
        public static void InjectBlock(BlockModel block)
        {
            BlockSheet.Stat statSheet =
                BlockSheet.Stat.GetDictionary()[block.blockID];

            Stats stats = block.stats;

            block.exp = statSheet.exp;

            stats.maxHp = statSheet.maxHp;
            block.hp = stats.calculatedMaxHp;

            stats.defense = statSheet.defense;
        }

        public static void InjectItem(ActorModel actor, ItemModel item)
        {
            if (item == null) return;
            actor.stats += item.stat;
        }

        public static void DejectItem(ActorModel actor, ItemModel item)
        {
            if (item == null) return;
            actor.stats -= item.stat;
        }
    }
}