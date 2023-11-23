using Minefarm.Entity.Actor;
using Minefarm.Entity.Bullet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.InGame.Level
{
    public static class LevelDB 
    {
        public static Stats GetBonusStat(int level)
        {
            var dic = LevelSheet.LevelExp.GetDictionary()[level];

            Stats ret = new Stats();
            ret.maxHp = dic.maxHp;
            ret.maxHpPercent = 0f;
            ret.attack = dic.attack;
            ret.attackPercent = 0f;
            ret.attackSpeed = dic.attackSpeed;
            ret.attackSpeedPercent = 0f;
            ret.attackRange = dic.attackRange;
            ret.attackRangePercent = 0f;
            ret.defense = dic.defense;
            ret.defensePercent = 0f;
            ret.defenseRatio = dic.defenseRatio;
            ret.durabilityNegation = dic.durabilityNegation;
            ret.criticalChance = dic.criticalChance;
            ret.criticalDamage = dic.criticalDamage;
            ret.speed = dic.speed;
            ret.speedPercent = 0f;
            ret.jumpPower = dic.jumpPower;
            return ret;
        }

        public static long GetNextExp(int level)
        {
            if (level == 0) level = 1;
            return LevelSheet.LevelExp.GetDictionary()[level].exp;
        }
    }
}

