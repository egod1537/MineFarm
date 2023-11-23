using Minefarm.Entity.Actor.EditorInspector;
using Minefarm.Entity.Bullet;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Entity.Actor.EditorInspector
{
    public class EditorActorModelStat : EditorActorModelPanel
    {
        public bool isFold;

        public EditorActorModelStat(ActorModel script) : base(script)
        {
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isFold = EditorGUILayout.Foldout(isFold, "[Actor Stat]");
                if (isFold)
                {
                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.level = EditorGUILayout.IntField("Level", script.level);
                        script.exp = EditorGUILayout.LongField("Exp", script.exp);
                        EditorGUILayout.LongField("Next Level Exp", script.GetNextLevelExp());
                        script.isGround = EditorGUILayout.Toggle("Is Ground", script.isGround);
                        script.isGround = EditorGUILayout.Toggle("Is Live", script.isLive);
                        script.isGround = EditorGUILayout.Toggle("Is Dead", script.isDead);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.gold = EditorGUILayout.IntField("Gold", script.gold);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.maxHp =
                            EditorGUILayout.IntField("Max Hp", script.stats.maxHp);
                        script.stats.maxHpPercent =
                            EditorGUILayout.FloatField("Max Hp Percent", script.stats.maxHpPercent);
                        EditorGUILayout.IntField("Calculated Max Hp", script.stats.calculatedMaxHp);
                        script.hp =
                            EditorGUILayout.IntField("Hp", script.hp);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.attack = EditorGUILayout.IntField("Attack", script.stats.attack);
                        script.stats.attackPercent = EditorGUILayout.FloatField("Attack Percent", script.stats.attackPercent);
                        EditorGUILayout.IntField("Calculated Attack", script.stats.calculatedAttack);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.attackSpeed = EditorGUILayout.FloatField("Attack Speed", script.stats.attackSpeed);
                        script.stats.attackSpeedPercent = EditorGUILayout.FloatField("Attack Speed Percent", script.stats.attackSpeedPercent);
                        EditorGUILayout.FloatField("Calculated Attack Speed",
                            script.stats.calculatedAttackSpeed);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.attackRange = EditorGUILayout.FloatField("Attack Range", script.stats.attackRange);
                        script.stats.attackRangePercent = EditorGUILayout.FloatField("Attack Range Percent", script.stats.attackRangePercent);
                        EditorGUILayout.FloatField("Calculated Attack Range",
                            script.stats.calculatedAttackRange);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.bulletModel =
                            (BulletModelType)EditorGUILayout.EnumPopup("Bullet Model",
                            script.stats.bulletModel);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.defense = EditorGUILayout.IntField("Defense", script.stats.defense);
                        script.stats.defensePercent = EditorGUILayout.FloatField("Defense Percent", script.stats.defensePercent);
                        EditorGUILayout.IntField("Calculated Defense", script.stats.calculatedDefense);

                        script.stats.defenseRatio = EditorGUILayout.FloatField("Defense Ratio", script.stats.defenseRatio);
                        script.stats.durabilityNegation = EditorGUILayout.FloatField("Durability Negation", script.stats.durabilityNegation);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.criticalChance = EditorGUILayout.Slider("Critical Chance", script.stats.criticalChance, 0f, 1f);
                        EditorGUILayout.FloatField("Calculated Critical Chance",
                            script.stats.calculatedCriticalChance);
                        script.stats.criticalDamage = EditorGUILayout.FloatField("Critical Damage", script.stats.criticalDamage);
                        EditorGUILayout.FloatField("Calculated Critical Damage",
                            script.stats.calculatedCriticalDamage);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.speed = EditorGUILayout.FloatField("Speed", script.stats.speed);
                        script.stats.speedPercent = EditorGUILayout.FloatField("Speed Percent", script.stats.speedPercent);
                        EditorGUILayout.FloatField("Calculated Speed", script.stats.calculatedSpeed);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.stats.jumpPower = EditorGUILayout.FloatField("Jump Power\",", script.stats.jumpPower);
                        EditorGUILayout.FloatField("Calculated Jump Power",
                            script.stats.calculatedJumpPower);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}

