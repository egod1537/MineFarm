using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenSlashPostProcessing))]
public class EditorScreenSlashPostProcessing : Editor
{
    ScreenSlashPostProcessing script;
    private void OnEnable()
    {
        script = (ScreenSlashPostProcessing)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Run")) script.Run();
    }
}
