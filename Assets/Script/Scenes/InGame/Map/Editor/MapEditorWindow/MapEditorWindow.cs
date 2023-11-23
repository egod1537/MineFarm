using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Minefarm.Map.Block;
using UniRx;
using UnityEditor.MPE;
using System.Security.Cryptography;

namespace Minefarm.Map.Window
{
    public class MapEditorWindow : EditorWindow
    {
        [MenuItem("Window/Map/Editor")]
        static void Open() {
            MapEditorWindow activeWindow = 
                (MapEditorWindow)GetWindow(typeof(MapEditorWindow), false, "Map Editor Tools");

            activeWindow.Show();
            activeWindow.OnEnable();
        }
        public enum EditToolType
        {
            Select,
            MagicWand,
            Brush,
            Replace
        }

        public MapModel map;
        private Transform transform { get => map.transform; }

        public EditToolType selectedTool;
        public Vector3Int selectedPosition;

        public List<MapEditorWindowHaviour> haviours = new();
        public MapEditorWindowInspectorViewer inspectorViwer;

        private Dictionary<KeyCode, bool> keyDownList = new();

        private void OnEnable()
        {
            foreach (var haviour in haviours) haviour.OnEnable();
            SceneView.duringSceneGui += OnSceneGUI;

            Reload();
        }

        private void OnDisable()
        {
            foreach (var haviour in haviours) haviour.OnDisable();
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            foreach (var haviour in haviours) haviour.OnGUI();
        }
        
        private void OnSceneGUI(SceneView sceneView)
        {
            if (Selection.activeGameObject != null)
            {
                MapModel target = Selection.activeGameObject.GetComponent<MapModel>();
                if (map != target) SetMap(target);
            }
            if (haviours.Count == 0) SetMap(map);

            ProcessEvent(Event.current);
            foreach (var haviour in haviours) haviour.OnSceneGUI(sceneView);

            sceneView.Repaint();
        }

        private void ProcessEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.KeyDown:
                    SetKeyState(e.keyCode, true);
                    break;
                case EventType.KeyUp:
                    SetKeyState(e.keyCode, false);
                    break;
            }
        }

        public bool IsKey(KeyCode key)
        {
            if (!keyDownList.ContainsKey(key)) keyDownList.Add(key, false);
            return keyDownList[key];
        }
        public void SetKeyState(KeyCode key, bool state)
        {
            if (!keyDownList.ContainsKey(key)) keyDownList.Add(key, false);
            keyDownList[key] = state;
        }

        public void SetMap(MapModel model)
        {
            this.map = model;

            haviours = new List<MapEditorWindowHaviour>()
            {
                new MapEditorWindowViewer(this),
                new MapEditorWindowInspectorViewer(this),
                new MapEditorWindowSelectTool(this),
                new MapEditorWindowBrushTool(this)
            };

            foreach (var haviour in haviours) haviour.OnEnable();
            Reload();
        }

        public void Reload()
        {
            if (map == null || map.mapData == null) return;
            map.Load();
        }
        private void AlignBlocks()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform tr = transform.GetChild(i);
                tr.localPosition = (tr.localPosition - Vector3.one * 0.5f).ToRound();
            }
        }
        public bool GetMapPositionFromScreenPoint(Vector3 screenPoint, out Vector3 ret)
        {
            if (GetWorldPositionFromScreenPoint(screenPoint, out ret))
                return false;
            ret = map.WorldToMapPosition(ret);
            return true;
        }
        public bool GetWorldPositionFromScreenPoint(Vector3 screenPoint, out Vector3 ret)
        {
            ret = -Vector3Int.one;
            if (map == null) return false;
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(screenPoint);

            int layer_map = 1 << LayerMask.NameToLayer("Map");
            int layer_block = 1 << LayerMask.NameToLayer("Block");
            if (Physics.Raycast(ray, out hit, layer_map | layer_block))
            {
                ret = hit.point;
                return true;
            }
            return false;
        }
        public bool GetMapIndexFromScreenPoint(Vector3 screenPoint, out Vector3Int ret)
        {
            ret = -Vector3Int.one;
            if (map == null) return false;
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(screenPoint);

            int layer_map = 1 << LayerMask.NameToLayer("Map");
            int layer_block = 1 << LayerMask.NameToLayer("Block");
            if (Physics.Raycast(ray, out hit, layer_map | layer_block))
            {
                ret = map.WorldToMapIndex(hit.point + Vector3.one * 0.5f);
                return true;
            }
            return false;
        }
        public bool GetMapDirectionFromScreenPoint(Vector3 screenPoint, out Vector3Int ret)
        {
            ret = -Vector3Int.one;
            if (map == null) return false;
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(screenPoint);

            int layer_map = 1 << LayerMask.NameToLayer("Map");
            int layer_block = 1 << LayerMask.NameToLayer("Block");
            if (Physics.Raycast(ray, out hit, layer_map | layer_block))
            {
                Vector3 diff = hit.point - hit.transform.position;

                diff.Normalize();
                if (diff.x >= diff.y && diff.x >= diff.z)
                    diff.y = diff.z = 0f;
                if (diff.y >= diff.x && diff.y >= diff.z)
                    diff.x = diff.z = 0f;
                if (diff.z >= diff.x && diff.z >= diff.y)
                    diff.x = diff.y = 0f;
                ret = diff.ToRound().ToVector3Int();
                return true;
            }
            return false;
        }
    }
}