using Minefarm.Map.Structure;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Entity.Actor.Monster.EditroInspector
{
    [CustomEditor(typeof(MonsterSpawner))]
    public class EditorMonsterSpawner : Editor
    {
        MonsterSpawner script;
        private void OnEnable()
        {
            script = (MonsterSpawner)target;
        }

        public override void OnInspectorGUI()
        {
            script.radius = EditorGUILayout.FloatField("Radius", script.radius);

            EditorGUILayout.LabelField("Quantity");
            EditorGUILayout.BeginHorizontal();
            {
                script.minQuantity = EditorGUILayout.IntField("Min", script.minQuantity);
                script.maxQuantity = EditorGUILayout.IntField("Max", script.maxQuantity);
            }

            script.period = EditorGUILayout.FloatField("Period", script.period);

            EditorGUILayout.EndHorizontal();
            {
                Rect rect = EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUI.ProgressBar(
                        rect, 
                        script.timer / script.period, 
                        $"Spawning ({script.timer} / {script.period})");
                    EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Spawn")) script.SpawnEntity();
        }
    }
}