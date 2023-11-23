using Minefarm.Entity.Bullet;
using Minefarm.Entity.EditorInsepctor;
using Minefarm.Inventory;
using NUnit.Framework;
using PlasticPipe.PlasticProtocol.Messages;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Entity.Actor.EditorInspector
{
    [CustomEditor(typeof(ActorModel), true)]
    public class EditorActorModel : EditorEntityModel
    {
        public bool isInventoryFold;
        protected new ActorModel script { get => base.script as ActorModel; }

        protected List<EditorActorModelPanel> panels;

        protected virtual void OnEnable()
        {
            base.OnEnable();
            panels = new();
            panels.Add(new EditorActorModelStat(script));
            panels.Add(new EditorActorModelInventory(script));

            foreach (var panel in panels) panel.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();
            foreach (var panel in panels) panel.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(script, $"{typeof(ActorModel)} {script.name}");
            PrefabUtility.RecordPrefabInstancePropertyModifications(script);
        }

        private void DrawStatField()
        {
            
        }

        int selectedSlot;

        private void DrawInventoryField()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isInventoryFold = EditorGUILayout.Foldout(isInventoryFold, "[Inventory]");
                if (isInventoryFold)
                {
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}