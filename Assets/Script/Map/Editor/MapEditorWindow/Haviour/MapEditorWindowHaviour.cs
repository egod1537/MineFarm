using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Minefarm.Map.Window
{
    public class MapEditorWindowHaviour
    {
        protected MapEditorWindow window;
        protected MapModel map { get => window.map; }
        protected Transform transform { get => map.transform; }

        public MapEditorWindowHaviour(MapEditorWindow window)
        {
            this.window = window;
        }

        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
        public virtual void OnGUI()
        {

        }
        public virtual void OnSceneGUI(SceneView sceneView)
        {

        }

        protected virtual bool IsActive()
            => window.map != null && window.map.mapData != null;
    }
}