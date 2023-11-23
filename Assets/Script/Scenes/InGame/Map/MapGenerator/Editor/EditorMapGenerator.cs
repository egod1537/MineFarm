using UnityEngine;
using UnityEditor;

namespace Minefarm.Map.Generator.EditorInspector
{
    [CustomEditor(typeof(MapGenerator), true)]
    public class EditorMapGenerator : Editor
    {
        MapGenerator generator;
        public void OnEnable()
        {
            generator = (MapGenerator)target;  
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Map Generator]");
                generator.target = (MapModel)EditorGUILayout.ObjectField(
                    "Target", generator.target, typeof(MapModel));

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Generate")) generator.Generate();
                    if (GUILayout.Button("Destroy All")) generator.DestroyAll();
                }
                EditorGUILayout.EndHorizontal();            
            }
            EditorGUILayout.EndVertical();
            base.OnInspectorGUI();
        }
    }
}