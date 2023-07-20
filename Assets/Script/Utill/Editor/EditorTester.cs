using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace EditorInspector
{
    [CustomEditor(typeof(Tester))]
    public class EditorTester : Editor
    {
        Tester script;
        private void OnEnable()
        {
            script= (Tester)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Action")) script.Action();
        }
    }
}