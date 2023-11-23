using Minefarm.Entity.Actor.Monster.Table;
using Minefarm.Item;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Minefarm.Entity.Actor.Monster.EditroInspector
{
    [CustomEditor(typeof(MonsterDropItemTable))]
    public class EditorMonsterDropItemTable : Editor
    {
        MonsterDropItemTable script;

        List<PercentagePanel> panels;
        private void OnEnable()
        {
            script = (MonsterDropItemTable)target;

            panels = new();
            foreach (var data in script.table.Values) 
                panels.Add(new PercentagePanel(data));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            string searchString = "";
            searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));

            foreach (var key in script.table.Keys)
            {
                Rect rect = EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField($"[{key}]", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    panels[(int)key].OnInsepctorPanel();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(script, $"{typeof(MonsterDropItemTable)} {script.name}");
            PrefabUtility.RecordPrefabInstancePropertyModifications(this.script);
        }

        public class PercentagePanel
        {
            DropItemTable table;

            Dictionary<ItemID, bool> isItemFold;

            ItemID selectedItemID;
            int selectedItemQuantity;
            bool isFold;
            public PercentagePanel(DropItemTable table)
            {
                this.table = table;
                this.isItemFold = new();
            }

            public void OnInsepctorPanel()
            {
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField($"Number of Drop items {table.Count}");

                    EditorGUILayout.BeginHorizontal();
                    {
                        selectedItemID = (ItemID)EditorGUILayout.EnumPopup(
                            "Item ID", selectedItemID);
                        if (GUILayout.Button("Add"))
                        {
                            if (!table.ContainsKey(selectedItemID))
                                table.Add(selectedItemID, new CountPercentageTable());
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    DrawCountPercentageTable(table);
                }
                EditorGUILayout.EndVertical();
            }

            private void DrawCountPercentageTable(DropItemTable table)
            {
                List<ItemID> itemIDs = new(table.Keys);
                
                foreach(var itemID in itemIDs)
                {
                    CountPercentageTable counter = table[itemID];

                    Rect rect = EditorGUILayout.BeginVertical("HelpBox");
                    {
                        if (!isItemFold.ContainsKey(itemID)) isItemFold.Add(itemID, false);
                        isItemFold[itemID] = EditorGUILayout.Foldout(isItemFold[itemID], $"[{(int)itemID}] {itemID}");
                        if (GUI.Button(
                            new Rect(rect.x + rect.width - 24, rect.y, 24, 24),
                            "-"))
                        {
                            table.Remove(itemID);
                            continue;
                        }

                        if (isItemFold[itemID])
                        {
                            DrawWeightTable(itemID, counter);
                            counter.Normalize();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            private void DrawWeightTable(ItemID itemID, CountPercentageTable counter)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    Texture texItem = ItemDB.GetItemIcon(itemID).texture;
                    EditorGUILayout.BeginVertical("Box");
                    {
                        GUILayout.Label(texItem, GUILayout.Width(64), GUILayout.Height(64));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal("Box");
                        {
                            selectedItemQuantity = EditorGUILayout.IntField("Drop Item Weight", selectedItemQuantity);
                            if (GUILayout.Button("Add"))
                            {
                                if (!counter.ContainsKey(selectedItemQuantity))
                                    counter.Add(selectedItemQuantity, 0.0f);
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        List<int> keys = new(counter.Keys);
                        foreach (var key in keys)
                        {
                            Rect rect = EditorGUILayout.BeginHorizontal("Box");
                            {
                                if (GUILayout.Button("-", GUILayout.Width(24), GUILayout.Height(24)))
                                {
                                    counter.Remove(key);
                                    continue;
                                }
                                counter[key] = EditorGUILayout.Slider($"Weight {key} : ", counter[key], 0.0f, 1.0f);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}