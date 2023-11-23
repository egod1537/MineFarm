using UnityEngine;
using UnityEditor;
namespace Minefarm.ItemSet.EditorInspector
{
    [CustomEditor(typeof(ItemIconCapture))]
    public class EditorItemIconCapture : Editor
    {
        ItemIconCapture script;
        private void OnEnable()
        {
            script = (ItemIconCapture)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField($"Width : {Screen.width}");
            EditorGUILayout.LabelField($"Height : {Screen.height}");
            if (GUILayout.Button("Capture")) script.Capture();
        }
    }
}

