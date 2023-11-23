using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.ItemSet.EditorInspector
{
    [CustomEditor(typeof(MultipleItemIconCapture))]
    public class EditorMultipleItemCapture : Editor
    {
        MultipleItemIconCapture script;
        private void OnEnable()
        {
            script = (MultipleItemIconCapture)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Capture")) script.MultipleCapture();
        }
    }
}