using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Entity.Actor.EditorInspector;

namespace Minefarm.Entity.Actor.Player.EditorInspector
{
    [CustomEditor(typeof(PlayerModel))]
    public class EditorPlayerModel : EditorActorModel
    {
        private bool isPlayerFold;
        protected new PlayerModel script { get => base.script as PlayerModel; }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isPlayerFold = EditorGUILayout.Foldout(isPlayerFold, "[Player Information]");
                if (isPlayerFold)
                {
                    script.dashCount =
                        EditorGUILayout.IntField("Dash Count", script.dashCount);
                    script.dashRecycleDelay =
                        EditorGUILayout.FloatField("Dash Recycle Delay", script.dashRecycleDelay);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(script);
        }
    }
}