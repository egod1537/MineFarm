using Minefarm.Entity.Bullet;
using Minefarm.Entity.EditorInsepctor;
using UnityEditor;

namespace Minefarm.Entity.Actor.EditorInspector
{
    [CustomEditor(typeof(ActorModel), true)]
    public class EditorActorModel : EditorEntityModel
    {
        public bool isActorFold;
        protected ActorModel script { get => base.script as ActorModel; }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isActorFold = EditorGUILayout.Foldout(isActorFold, "[Actor Information]");
                if (isActorFold)
                {
                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.level = EditorGUILayout.IntField("Level", script.level);
                        script.exp = EditorGUILayout.IntField("Exp", script.exp);
                        script.isGround = EditorGUILayout.Toggle("Is Ground", script.isGround);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.maxHp =
                            EditorGUILayout.IntField("Max Hp", script.maxHp);
                        script.maxHpPercent =
                            EditorGUILayout.FloatField("Max Hp Percent", script.maxHpPercent);
                        EditorGUILayout.IntField("Calculated Max Hp", script.calculatedMaxHp);
                        script.hp =
                            EditorGUILayout.IntField("Hp", script.hp);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.attack =
                            EditorGUILayout.IntField("Attack", script.attack);
                        script.attackPercent =
                            EditorGUILayout.FloatField("Attack Percent", script.attackPercent);
                        EditorGUILayout.IntField("Calculated Attack", script.calculatedAttack);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.attackSpeed =
                            EditorGUILayout.FloatField("Attack Speed", script.attackSpeed);
                        script.attackSpeedPercent =
                            EditorGUILayout.FloatField("Attack Speed Percent", script.attackSpeedPercent);
                        EditorGUILayout.FloatField("Calculated Attack Speed", script.calculatedAttackSpeed);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.attackRange =
                            EditorGUILayout.FloatField("Attack Range", script.attackRange);
                        script.attackRangePercent =
                            EditorGUILayout.FloatField("Attack Range Percent", script.attackRangePercent);
                        EditorGUILayout.FloatField("Calculated Attack Range", script.calculatedAttackRange);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.bulletModel =
                            (BulletModelType)EditorGUILayout.EnumPopup("Bullet Model", script.bulletModel);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.defense =
                            EditorGUILayout.IntField("Defense", script.defense);
                        script.defensePercent =
                            EditorGUILayout.FloatField("Defense Percent", script.defensePercent);
                        EditorGUILayout.IntField("Calculated Defense", script.calculatedDefense);

                        script.defenseRatio =
                            EditorGUILayout.FloatField("Defense Ratio", script.defenseRatio);
                        script.durabilityNegation =
                            EditorGUILayout.FloatField("Durability Negation", script.durabilityNegation);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.criticalChance =
                            EditorGUILayout.Slider("Critical Chance", script.criticalChance, 0f, 1f);
                        script.criticalDamage =
                            EditorGUILayout.FloatField("Critical Damage", script.criticalDamage);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.speed =
                            EditorGUILayout.FloatField("Speed", script.speed);
                        script.speedPercent =
                            EditorGUILayout.FloatField("Speed Percent", script.speedPercent);
                        EditorGUILayout.FloatField("Calculated Speed", script.calculatedSpeed);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        script.jumpPower =
                            EditorGUILayout.FloatField("Jump", script.jumpPower);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}