using Minefarm.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Entity.Actor.EditorInspector
{
    public class EditorActorModelInventory : EditorActorModelPanel
    {
        bool isFold;
        ItemCategory selectedCategory;
        int selectedSlot;

        GUIContent[] contentCategory;

        public EditorActorModelInventory(ActorModel script) : base(script)
        {
            contentCategory = new GUIContent[Enum.GetValues(typeof(ItemCategory)).Length];
            foreach (var category in Enum.GetValues(typeof(ItemCategory)))
                contentCategory[(int)category] = new GUIContent(category.ToString());
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isFold = EditorGUILayout.Foldout(isFold, "[Actor Inventory]");
                if (isFold)
                {
                    EditorGUILayout.BeginVertical("Box");
                    {
                        selectedCategory = (ItemCategory)GUILayout.SelectionGrid(
                        (int)selectedCategory,
                        contentCategory,
                        4);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    {
                        DrawInventorySlot(selectedCategory);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawInventorySlot(ItemCategory category)
        {
            Inventory.InventoryModel.ItemList items = script.inventory[category];
            GUIContent[] contentItem = new GUIContent[items.Count];
            for(int i=0; i < items.Count; i++)
            {
                contentItem[i] = new GUIContent();
                if (items[i] == null) continue;
                Texture texture = ItemDB.GetItemIcon(items[i].itemID).texture.ResizeTexture(64, 64);
                GUIContent content = new GUIContent(texture);
                content.tooltip = $"Count : {items[i].count}";
                contentItem[i] = content;
            }

            selectedSlot = GUILayout.SelectionGrid(selectedSlot, contentItem, 6);
        }
    }
}

