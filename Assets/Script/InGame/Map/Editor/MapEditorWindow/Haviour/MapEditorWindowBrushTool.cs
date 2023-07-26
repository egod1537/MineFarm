using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Minefarm.Entity.Actor.Block;

namespace Minefarm.Map.Window
{
    public class MapEditorWindowBrushTool : MapEditorWindowHaviour
    {
        const int SIZE_BLOCK_ICON = 32;

        private Dictionary<BlockID, GameObject> blocks;
        private GUIContent[] blockContents;
        public MapEditorWindowBrushTool(MapEditorWindow window) : base(window)
        {
            blocks = new Dictionary<BlockID, GameObject>();
            blockContents = new GUIContent[Enum.GetValues(typeof(BlockID)).Length];

            foreach(BlockID id in Enum.GetValues(typeof(BlockID)))
            {
                blocks.Add(id, BlockDB.LoadBlock(id));

                Texture texture = 
                    ResizeTexture(AssetPreview.GetAssetPreview(blocks[id]), 
                    SIZE_BLOCK_ICON, 
                    SIZE_BLOCK_ICON);
                GUIContent content = new GUIContent(texture);

                blockContents[(int)id] = content;
            }
        }
        BlockID selectedBlock;
        protected override  bool IsActive() 
            => base.IsActive() && window.selectedTool == MapEditorWindow.EditToolType.Brush;
        public override void OnGUI()
        {
            if (!IsActive()) return;

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Palette]", EditorStyles.boldLabel);
                EditorGUILayout.Space(10);
                selectedBlock = (BlockID)GUILayout.SelectionGrid(
                    (int)selectedBlock, 
                    blockContents, 
                    (int) window.position.width/ SIZE_BLOCK_ICON / 2);
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            if (!IsActive()) return;

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    if(e.keyCode == KeyCode.A)
                    {
                        BlockModel block = window.map.CreateBlock(window.selectedPosition, selectedBlock);
                        e.Use();
                    }
                    if(e.keyCode == KeyCode.D)
                    {
                        window.map.DestroyBlock(window.selectedPosition);
                        e.Use();
                    }
                    break;
            }
        }

        private Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
        {
            RenderTexture rt = new RenderTexture(newWidth, newHeight, 0);
            Graphics.Blit(source, rt);
            Texture2D newTexture = new Texture2D(newWidth, newHeight);
            newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            newTexture.Apply();
            RenderTexture.active = null;
            rt.Release();
            return newTexture;
        }
    }
}