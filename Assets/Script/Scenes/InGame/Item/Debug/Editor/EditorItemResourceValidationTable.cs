using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Minefarm.Item.EditorInspector
{
    [CustomEditor(typeof(ItemResourceValidationTable))]
    public class EditorItemResourceValidationTable : Editor
    {
        ItemResourceValidationTable script;
        private void OnEnable()
        {
            script = (ItemResourceValidationTable)target;
        }

        bool isFoldCorrect;
        bool isFoldNot;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Validate")) script.Validate();

            isFoldCorrect = EditorGUILayout.Foldout(isFoldCorrect, "[Correct Items]");
            if (isFoldCorrect)
            {
                EditorGUILayout.BeginVertical("Box");
                {
                    foreach (var val in script.table)
                    {
                        ItemID id = val.Key;
                        ItemResourceTable table = val.Value;
                        if (table.IsCorrect()) DrawItemResourceTable(id, table);
                    }
                }
                EditorGUILayout.EndVertical();
            }

            isFoldNot = EditorGUILayout.Foldout(isFoldNot, "[Not Resources Items]");
            if (isFoldNot)
            {
                EditorGUILayout.BeginVertical("Box");
                {
                    foreach (var val in script.table)
                    {
                        ItemID id = val.Key;
                        ItemResourceTable table = val.Value;
                        if (!table.IsCorrect()) DrawItemResourceTable(id, table);
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawItemResourceTable(ItemID id, ItemResourceTable table)
        {

            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField($"[Item] {(int)id} {id}", EditorStyles.boldLabel);
                    if (table.isStat)
                        EditorGUILayout.EnumPopup("Item Category", table.category);
                }
                EditorGUILayout.EndVertical();

                LabelValidation("Stat Table", table.isStat);
                LabelValidation("Attribute Table", table.isAttribute);

                LabelValidation("Item Model", table.isItemModel);
                LabelValidation("Item Icon", table.isItemIcon);

            }
            EditorGUILayout.EndVertical();
        }

        private void LabelValidation(string prefix, bool check)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"{prefix} : ");

                GUIStyleState styleState = new GUIStyleState()
                {
                    textColor = check ? Color.green : Color.red
                };
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    normal = styleState
                };

                string str = check ? "Correct" : "Not Resource";
                EditorGUILayout.LabelField($"{str}", style);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

