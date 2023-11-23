using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorInspector
{
    [CustomEditor(typeof(ColorGroup))]
    public class EditorColorGroup : Editor
    {
        ColorGroup script;
        private void OnEnable()
        {
            script = (ColorGroup)target;    
        }

        public override void OnInspectorGUI()
        {
            script.color = EditorGUILayout.ColorField("Color", script.color);
        }
    }
}