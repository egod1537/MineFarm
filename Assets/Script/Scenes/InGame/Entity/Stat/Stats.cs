using Minefarm.Entity.Actor;
using Minefarm.Entity.Bullet;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

namespace Minefarm.Entity.Actor
{
    [Serializable]
    public class Stats
    {
        public int maxHp;
        public float maxHpPercent = 1.0f;
        public int calculatedMaxHp { get => Mathf.RoundToInt(maxHp * maxHpPercent); }

        public int attack;
        public float attackPercent = 1.0f;
        public int calculatedAttack { get => Mathf.RoundToInt(attack * attackPercent); }

        public float attackSpeed = 1.0f;
        public float attackSpeedPercent = 1.0f;
        public float calculatedAttackSpeed { get => attackSpeed * attackSpeedPercent; }

        public float attackRange = 1f;
        public float attackRangePercent = 1.0f;
        public float calculatedAttackRange { get => attackRange * attackRangePercent; }

        public BulletModelType bulletModel = BulletModelType.Melee;

        public int defense;
        public float defensePercent = 1.0f;
        public int calculatedDefense { get => Mathf.RoundToInt(defense * defensePercent); }

        public float defenseRatio;
        public float calculatedDefenseRatio { get => defenseRatio; }
        public float durabilityNegation;
        public float calculatedDurabilityNegation { get => durabilityNegation; }

        public float criticalChance;
        public float calculatedCriticalChance { get => criticalChance; }
        public float criticalDamage = 2.0f;
        public float calculatedCriticalDamage { get => criticalDamage; }

        public float speed = 1.0f;
        public float speedPercent = 1.0f;
        public float calculatedSpeed { get => speed * speedPercent; }

        public float jumpPower = 1.0f;
        public float calculatedJumpPower { get => jumpPower; }

        public Stats Clone()
        {
            Stats ret = new Stats();
            ret.maxHp = maxHp;
            ret.maxHpPercent = maxHpPercent;
            ret.attack = attack;
            ret.attackPercent = attackPercent;
            ret.attackSpeed = attackSpeed;
            ret.attackSpeedPercent = attackSpeedPercent;
            ret.attackRange = attackRange;
            ret.attackRangePercent = attackRangePercent;
            ret.bulletModel = bulletModel;
            ret.defense = defense;
            ret.defensePercent = defensePercent;
            ret.defenseRatio = defenseRatio;
            ret.durabilityNegation = durabilityNegation;
            ret.criticalChance = criticalChance;
            ret.criticalDamage = criticalDamage;
            ret.speed = speed;
            ret.speedPercent = speedPercent;
            ret.jumpPower= jumpPower;
            return ret;
        }

        public int FormulateAttack(float percent = 1.0f)
        {
            int ret = calculatedAttack;
            ret = Mathf.RoundToInt(ret * percent);
            return ret;
        }

        public int FormulateDamage(ActorModel actor, int damage, out bool isCritical)
        {
            isCritical = false;
            if (UnityEngine.Random.Range(0f, 1f) < actor.stats.calculatedCriticalChance)
            {
                damage = Mathf.RoundToInt(actor.stats.calculatedCriticalDamage * damage);
                isCritical = true;
            }

            float negation = 1.0f - actor.stats.calculatedDefenseRatio * (1 - durabilityNegation);
            damage = Mathf.RoundToInt(damage * negation);
            damage = Mathf.Max(0, damage - calculatedDefense);
            return Mathf.Max(1, damage);
        }
        public int FormulateDamage(ActorModel actor, int damage)
        {
            bool temp = false;
            return FormulateDamage(actor, damage, out temp);
        }

        public static Stats operator+(Stats stats, Stats other)
        {
            Stats ret = new Stats();
            ret.maxHp = stats.maxHp + other.maxHp;
            ret.maxHpPercent = stats.maxHpPercent + other.maxHpPercent;
            ret.attack = stats.attack + other.attack;
            ret.attackPercent = stats.attackPercent+ other.attackPercent;
            ret.attackSpeed = stats.attackSpeed+ other.attackSpeed;
            ret.attackSpeedPercent = stats.attackSpeedPercent + other.attackSpeedPercent;
            ret.attackRange = stats.attackRange + other.attackRange;
            ret.attackRangePercent = stats.attackRangePercent + other.attackRangePercent;
            ret.bulletModel = stats.bulletModel;
            ret.defense = stats.defense + other.defense;
            ret.defensePercent = stats.defensePercent + other.defensePercent;
            ret.defenseRatio = stats.defenseRatio + other.defenseRatio;
            ret.durabilityNegation = stats.durabilityNegation + other.durabilityNegation;
            ret.criticalChance = stats.criticalChance + other.criticalChance;
            ret.criticalDamage = stats.criticalDamage + other.criticalDamage;
            ret.speed = stats.speed + other.speed;
            ret.speedPercent = stats.speedPercent + other.speedPercent;
            ret.jumpPower = stats.jumpPower + other.jumpPower;
            return ret;
        }
        public static Stats operator -(Stats stats, Stats other)
        {
            Stats ret = new Stats();
            ret.maxHp = stats.maxHp - other.maxHp;
            ret.maxHpPercent = stats.maxHpPercent - other.maxHpPercent;
            ret.attack = stats.attack - other.attack;
            ret.attackPercent = stats.attackPercent - other.attackPercent;
            ret.attackSpeed = stats.attackSpeed - other.attackSpeed;
            ret.attackSpeedPercent = stats.attackSpeedPercent - other.attackSpeedPercent;
            ret.attackRange = stats.attackRange - other.attackRange;
            ret.attackRangePercent = stats.attackRangePercent - other.attackRangePercent;
            ret.bulletModel = stats.bulletModel;
            ret.defense = stats.defense - other.defense;
            ret.defensePercent = stats.defensePercent - other.defensePercent;
            ret.defenseRatio = stats.defenseRatio - other.defenseRatio;
            ret.durabilityNegation = stats.durabilityNegation - other.durabilityNegation;
            ret.criticalChance = stats.criticalChance - other.criticalChance;
            ret.criticalDamage = stats.criticalDamage - other.criticalDamage;
            ret.speed = stats.speed - other.speed;
            ret.speedPercent = stats.speedPercent - other.speedPercent;
            ret.jumpPower = stats.jumpPower - other.jumpPower;
            return ret;
        }
    }
}