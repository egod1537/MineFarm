using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Map.Generator.EditorInspector;

namespace Minefarm.Map.Generator.WaveFunctionCollapse.EditorInspector
{
    [CustomEditor(typeof(WaveFunctionCollapseMapGenerator))]
    public class EditorWaveFunctionCollapseMapGenerator : EditorMapGenerator
    {
        WaveFunctionCollapseMapGenerator script;
        private void OnEnable()
        {
            base.OnEnable();
            script = (WaveFunctionCollapseMapGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Process")) script.Process();
        }
    }
}