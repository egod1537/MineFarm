using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Map.Window;
using Codice.Client.BaseCommands;
using Minefarm.Map.Block;
using System;
using Minefarm.Entity.Actor.Block;

namespace Minefarm.Map.EditorInspector
{
    [CustomEditor(typeof(MapModel))]
    public class EditorMapModel : Editor
    {
        MapModel script;
        private void OnEnable()
        {
            script = (MapModel)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal("Box");
            {
                script.mapDataPath = EditorGUILayout.TextField("Path", script.mapDataPath);
                if (GUILayout.Button("Save")) script.Save();
                if (GUILayout.Button("Load")) script.Load();
            }
            EditorGUILayout.EndHorizontal();

            if (script.mapData != null)
            {
                script.size = EditorGUILayout.Vector3IntField("Size", script.size);

                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField("[Block List]", EditorStyles.boldLabel);
                    List<KeyValuePair<Vector3Int, BlockModel>> list = new();
                    foreach (var keyValue in script.blockModels) list.Add(keyValue);
                    list.Sort((x, y) =>
                    {
                        Vector3Int vx = x.Key, vy = y.Key;
                        if(vx.x != vy.x) return vx.x - vy.x;
                        if(vx.y != vy.y) return vx.y - vy.y;
                        return vx.z - vy.z;
                    });
                    foreach (var keyValue in list)
                    {
                        Vector3Int pos = keyValue.Key;
                        BlockModel block = null;
                        if (script.blockModels.ContainsKey(pos)) block = script.blockModels[pos];
                        EditorGUILayout.BeginVertical("HelpBox");
                        {
                            if (block == null)
                                EditorGUILayout.LabelField($"({pos.x}, {pos.y}, {pos.z})");
                            else
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

            serializedObject.ApplyModifiedProperties();
        }
    }
}
