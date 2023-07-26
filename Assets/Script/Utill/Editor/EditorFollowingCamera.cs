using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Utilty;

namespace Minefarm.Utilty.EditorInspector
{
    [CustomEditor(typeof(FollwingCamera))]
    public class EditorFollowingCamera : Editor
    {
        FollwingCamera script;
        private void OnEnable()
        {
            script = (FollwingCamera)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Follow"))
                script.transform.position = script.GetFollowingPosition();
        }
    }

}