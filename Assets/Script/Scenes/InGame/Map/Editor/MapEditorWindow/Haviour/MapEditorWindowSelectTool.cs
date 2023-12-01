using Codice.Client.BaseCommands;
using Minefarm.Entity.Actor.Block;
using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Minefarm.Map.Window
{
    public class MapEditorWindowSelectTool : MapEditorWindowHaviour
    {
        private HashSet<Vector3Int> nowSelected = new HashSet<Vector3Int>();

        public MapEditorWindowSelectTool(MapEditorWindow window) : base(window)
        {
        }

        protected override bool IsActive()
            => base.IsActive() && window.selectedTool == MapEditorWindow.EditToolType.Select;

        public override void OnSceneGUI(SceneView sceneView)
        {
            window.selectedPosition = GetSelectedPosition();
            if (window.selectedPosition != -Vector3Int.one)
                DrawCube(GetSelectedPosition());

            if (!IsActive()) return;

            ProcessEvent(Event.current, sceneView);
            DrawSelectedBlock();
        }
        
        private void ProcessEvent(Event e, SceneView sceneView)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if(e.button == 0)
                    {
                        Vector3Int pos = GetSelectedPosition();
                        if (map.blocks.ContainsKey(pos))
                        {
                            if (window.IsKey(KeyCode.LeftShift))
                            {
                                if (pos != -Vector3Int.one && !nowSelected.Contains(pos))
                                    nowSelected.Add(pos);
                            }
                            else
                            {
                                nowSelected.Clear();
                                nowSelected.Add(pos);
                            }
                            window.Repaint();
                            e.Use();
                            Selection.activeGameObject = map.gameObject;
                        }
                    }
                    break;
                case EventType.KeyDown:
                    if(nowSelected.Count >0 && e.keyCode == KeyCode.Delete)
                    {
                        foreach (var pos in nowSelected)
                            map.DestroyBlock(pos);
                        nowSelected.Clear();
                        e.Use();
                    }
                    break;
            }
        }

        private void DrawSelectedBlock()
        {
            Handles.matrix = map.transform.ToMat();
            Handles.color = Color.yellow;

            foreach (var selectedPos in nowSelected)
                Handles.DrawWireCube(selectedPos + Vector3.one * 0.5f, Vector3.one);
        }

        public override void OnGUI()
        {
            if (!IsActive()) return;

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Selected Infomation]", EditorStyles.boldLabel);
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Left Shift + Mouse Left Click : multiple selected");
                EditorGUILayout.LabelField("Delete : remove blocks selected");

                foreach (var keyValue in nowSelected)
                {
                    Vector3Int pos = keyValue;
                    BlockModel block = map.blockModels[pos];
                    EditorGUILayout.BeginVertical("HelpBox");
                    {
                        EditorGUILayout.ObjectField(
                            $"({pos.x}, {pos.y}, {pos.z}) [{(int)block.blockID}] {block.blockID}",
                            block.gameObject,
                            typeof(GameObject));
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        public Vector3Int GetSelectedPosition()
        {
            Vector3 mousePosition = Event.current.mousePosition;

            Vector3Int ret = -Vector3Int.one;
            if (!window.GetMapIndexFromScreenPoint(mousePosition, out ret))
                return ret;

            if(!(ret.y == 0 || map.blocks.Count == 0 || map.blocks.ContainsKey(ret)))
            {
                Vector3 to = Vector3.zero;
                if (!window.GetWorldPositionFromScreenPoint(mousePosition, out to))
                    return ret;
                ret = InterpolateSelectedPosition(to);
            }

            if (window.IsKey(KeyCode.LeftShift))
            {
                Vector3Int to = Vector3Int.zero;
                if (!window.GetMapDirectionFromScreenPoint(mousePosition, out to))
                    return -Vector3Int.one;
                ret += to;
            }
            return ret;
        }
        private Vector3Int InterpolateSelectedPosition(Vector3 pos)
        {
            if (!base.IsActive()) return pos.ToVector3Int();
            Vector3Int ret = map.size;
            foreach (var block in map.blockModels)
            {
                if (ret == map.size) ret = block.Key;
                else if ((pos - block.Value.transform.position).sqrMagnitude <
                    (pos - map.blockModels[ret].transform.position).sqrMagnitude)
                    ret = block.Key;
            }
            return ret;
        }
        private void DrawCube(Vector3 pos)
        {
            Handles.color = Color.red;
            Handles.matrix = map.transform.ToMat();
            Handles.DrawWireCube(pos+Vector3.one*0.5f, Vector3.one);
            Handles.Label(pos+Vector3Int.one, $"{pos.ToVector3Int()}");
        }
    }
}

