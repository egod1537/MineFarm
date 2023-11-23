using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Entity.Actor.EditorInspector;
using Minefarm.Map.Algorithm.Culling;

namespace Minefarm.Entity.Actor.Block.EditorInspector
{
    [CustomEditor(typeof(BlockModel), true)]
    public class EditorBlockModel : EditorActorModel
    {
        protected new BlockModel script { get => base.script as BlockModel; }
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            DrawBlockInformationPanel();

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(script, $"{typeof(EntityModel)} {script.name}");
            PrefabUtility.RecordPrefabInstancePropertyModifications(this.script);
        }

        bool isFold;

        private void DrawBlockInformationPanel()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isFold = EditorGUILayout.Foldout(isFold, "[Block Information]");

                script.pos = EditorGUILayout.Vector3IntField("Position", script.pos);
                script.blockID = (BlockID)EditorGUILayout.EnumPopup("Block ID", script.blockID);

                script.visible.Value =
                    EditorGUILayout.Toggle("Camera Visible", script.visible.Value);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}

