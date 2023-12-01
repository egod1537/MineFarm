using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Map.Algorithm.Culling.EditorInspector
{
    public class EditorMapCullingPanel
    {
        MapModel script;
        MapCulling culling { get => script.culling; }

        public EditorMapCullingPanel(MapModel script)
        {
            this.script = script;
        }

        bool isFold;
        public void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUI.indentLevel++;
                isFold = EditorGUILayout.Foldout(isFold, "[Map Culling]");
                if (isFold)
                {
                    DrawCullingPanel();
                }
                
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawCullingPanel()
        {
            DrawOcclusionPanel();
            DrawFrustumPanel();
        }

        private void DrawOcclusionPanel()
        {
            culling.activeOcclusion.Value =
                EditorGUILayout.Toggle("Occlusion", culling.activeOcclusion.Value);
        }
        private void DrawFrustumPanel()
        {
            culling.activeFrustum.Value =
                EditorGUILayout.Toggle("Frustum", culling.activeFrustum.Value);

            if (!culling.activeFrustum.Value) return;

            culling.frustum.size =
                EditorGUILayout.Vector3IntField("Frustum Size", culling.frustum.size);
        }
    }
}