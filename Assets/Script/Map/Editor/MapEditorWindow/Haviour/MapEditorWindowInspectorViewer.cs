using Codice.Client.BaseCommands;
using Minefarm.Map.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using static Minefarm.Map.Window.MapEditorWindow;

namespace Minefarm.Map.Window
{
    public class MapEditorWindowInspectorViewer : MapEditorWindowHaviour
    {
        string[] editorToolTypeNames;

        public MapEditorWindowInspectorViewer(MapEditorWindow window) : base(window)
        {
        }

        public override void OnEnable()
        {
            int len = Enum.GetValues(typeof(EditToolType)).Length;
            editorToolTypeNames = new string[len];
            for (int i = 0; i < len; i++)
                editorToolTypeNames[i] = $"{(EditToolType)i}";
        }

        public override void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            {
                window.map = (MapModel)
                    EditorGUILayout.ObjectField("Map Model", map, typeof(MapModel));
                if (GUILayout.Button("Apply")) window.SetMap(map);
                if (GUILayout.Button("Reload")) window.Reload();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Edit Tools]");
                window.selectedTool = (EditToolType)GUILayout.Toolbar(
                    (int)window.selectedTool, editorToolTypeNames);
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            ProcessEvent(sceneView);
        }

        private void ProcessEvent(SceneView sceneView)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    ProcessShortcut(e.keyCode);
                    break;
            }
        }
        private void ProcessShortcut(KeyCode key)
        {
            if (!window.IsKey(KeyCode.LeftShift)) return;
            switch (key)
            {
                case KeyCode.Q:
                    window.selectedTool = EditToolType.Select;
                    window.Repaint();
                    break;
                case KeyCode.W:
                    window.selectedTool = EditToolType.MagicWand;
                    window.Repaint();
                    break;
                case KeyCode.E:
                    window.selectedTool = EditToolType.Brush;
                    window.Repaint();
                    break;
                case KeyCode.R:
                    window.selectedTool = EditToolType.Replace;
                    window.Repaint();
                    break;
            }
        }
    }
}
