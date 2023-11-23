using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Entity.Actor.Monster.Controller.EditorInspector
{
    [CustomEditor(typeof(MonsterController))]
    public class EditorMonsterController : Editor
    {
        MonsterController script;
        private void OnEnable()
        {
            script = (MonsterController)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            script.target = (ActorModel)
                EditorGUILayout.ObjectField("Target", script.target, typeof(ActorModel));
            DrawMovePanel();
            DrawFindTargetPanel();

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(script, $"{typeof(MonsterController)} {script.name}");
            PrefabUtility.RecordPrefabInstancePropertyModifications(script);
        }

        private void DrawMovePanel()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Move Settings]", EditorStyles.boldLabel);
                script.distanceEpsilon = EditorGUILayout.Slider(
                    "Distance Epsilon", script.distanceEpsilon, 0.0f, 1.0f);
                script.maxMoveRotatingAngle = EditorGUILayout.FloatField("Max Move Rotation Angle",
                    script.maxMoveRotatingAngle);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFindTargetPanel()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Find Target Settings]", EditorStyles.boldLabel);
                script.distanceFollwoingTarget = EditorGUILayout.FloatField(
                    "Distance Follwoing Target", script.distanceFollwoingTarget);
                script.delayFindTarget = EditorGUILayout.FloatField(
                    "Delay Find Target", script.delayFindTarget);
                script.delayStartMissingTarget = EditorGUILayout.FloatField(
                    "Delat Start Missing Target", script.delayStartMissingTarget);
                script.delayMissingTarget = EditorGUILayout.FloatField(
                    "Delay Missing Target", script.delayMissingTarget);
            }
            EditorGUILayout.EndVertical();
        }
    }
}