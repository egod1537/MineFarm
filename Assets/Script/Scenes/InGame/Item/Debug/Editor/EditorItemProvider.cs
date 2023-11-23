using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Item.EditorInspector
{
    [CustomEditor(typeof(ItemProvider))]
    public class EditorItemProvider : Editor
    {
        ItemProvider script;
        private void OnEnable()
        {
            script = (ItemProvider)target;  
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Provide")) script.Provide();
        }
    }
}