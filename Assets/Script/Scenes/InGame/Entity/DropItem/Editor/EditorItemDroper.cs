using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Entity.DropItem.EdtiorInspector
{
    [CustomEditor(typeof(ItemDroper))]
    public class EditorItemDroper : Editor
    {
        ItemDroper script;
        private void OnEnable()
        {
            script = target as ItemDroper;  
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Drop")) script.Drop();
        }
    }
}