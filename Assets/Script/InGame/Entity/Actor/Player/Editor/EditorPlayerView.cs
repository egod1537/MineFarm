using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Minefarm.Entity.Item.Equipment;

namespace Minefarm.Entity.Actor.Player.EditorInspector
{
    [CustomEditor(typeof(PlayerView))]
    public class EditorPlayerView : Editor
    {
        PlayerView script;
        private void OnEnable()
        {
            script= (PlayerView)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Equipment Parents]", EditorStyles.boldLabel);
                foreach(EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
                {
                    if (type == EquipmentType.None) continue;
                    if (!script.equipmentTransform.ContainsKey(type))
                        script.equipmentTransform.Add(type, null);
                    script.equipmentTransform[type] =
                        (Transform)EditorGUILayout.ObjectField(
                            $"{type}",
                            script.equipmentTransform[type], 
                            typeof(Transform),
                            true);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}