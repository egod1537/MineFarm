using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Map.Algorithm.SPFA.EditorInspector
{
    public class EditorMapSPFAPanel
    {
        MapModel model;
        MapSPFA spfa { get => model.pathFinder; }
        MapSPFAViewer viewer { get => spfa.viewer; }

        bool isFoldPanel;
        public EditorMapSPFAPanel(MapModel model)
        {
            this.model = model;
        }

        public void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUI.indentLevel++;
                isFoldPanel = EditorGUILayout.Foldout(isFoldPanel, "[SPFA Panel]");
                if (isFoldPanel)
                {
                    DrawSPFASetting();
                    DrawSPFAViewerSetting();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSPFASetting()
        {
            EditorGUILayout.LabelField("[SPFA Setting]", EditorStyles.boldLabel);
            spfa.resolution = EditorGUILayout.IntField("Resolution", spfa.resolution);
            spfa.start = EditorGUILayout.Vector3Field("Start", spfa.start); 
            spfa.destination = EditorGUILayout.Vector3Field("Destination", spfa.destination);
            if (GUILayout.Button("Query")) spfa.Query();
        }
        private void DrawSPFAViewerSetting()
        {
            EditorGUILayout.LabelField("[SPFA Viewer Setting]", EditorStyles.boldLabel);
            viewer.isMapField = EditorGUILayout.Toggle("View Map Field", viewer.isMapField);
            viewer.isMapWall = EditorGUILayout.Toggle("View Map Wall", viewer.isMapWall);
            viewer.isMapRoute = EditorGUILayout.Toggle("View Map Route", viewer.isMapRoute);
        }
    }
}
