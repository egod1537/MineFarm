using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Map.EditorInspector
{
    [CustomEditor(typeof(MapAlgorithmExecuter))]
    public class EditorMapAlogrithmExecuter : Editor
    {
        MapAlgorithmExecuter script;
        private void OnEnable()
        {
            script= (MapAlgorithmExecuter)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal("[Short Path]");
            {
                if (GUILayout.Button("Action")) script.ShortestPath();
                if (GUILayout.Button("Clear")) script.ClearShortestPath();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

