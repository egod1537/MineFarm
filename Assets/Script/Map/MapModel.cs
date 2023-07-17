using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UIElements;
using UnityEditor.Build.Content;
using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using static Minefarm.Map.MapData;

namespace Minefarm.Map
{
    [RequireComponent(typeof(BoxCollider))]
    public class MapModel : MonoBehaviour
    {
        string PATH_MAPDATA { get => Application.dataPath + "/Resources/MapData"; }

        public Subject<Vector3Int> onResize = new Subject<Vector3Int>();

        public string mapDataPath;
        public MapData mapData;

        public Vector3Int size
        {
            get => mapData.size;
            set => Resize(value);
        }

        [SerializeField]
        public BlockDictionary blocks { 
            get => mapData.blocks; 
            set => mapData.blocks = value;
        }

        public Dictionary<Vector3Int, BlockModel> blockModels
            = new Dictionary<Vector3Int, BlockModel>();

        private BoxCollider _boxCollider;
        public BoxCollider boxCollider { get => GetComponent<BoxCollider>(); }
        public void OnEnable()
        {
            Load();    
        }

        public void Resize(Vector3Int resize)
        {
            if (mapData == null) return;
            mapData.size = resize;
            boxCollider.size = new Vector3(mapData.size.x, 0f, mapData.size.z);
            boxCollider.center = new Vector3(mapData.size.x*0.5f, 0f, mapData.size.z*0.5f);
            onResize.OnNext(resize);
        }

        public void Save()
        {
            if (mapDataPath.IsUnityNull() || mapData == null) return;
            JsonDB.SaveJsonFile($"{PATH_MAPDATA}/{mapDataPath}", mapData);
        }

        public void Load()
        {
            if (mapDataPath.IsUnityNull()) return;
            mapData = JsonDB.LoadJsonFile<MapData>($"{PATH_MAPDATA}/{mapDataPath}");
            if (mapData == null) mapData = new MapData();
            Dictionary<Vector3Int, BlockID> db = new(blocks);
            blockModels = new();

            transform.DestroyImmediateChild();

            foreach (var block in db)
                CreateBlock(block.Key, block.Value);
        }

        public bool InArea(Vector3Int pos)
            => 0 <= pos.x && pos.x < size.x && 
            0 <= pos.y && pos.y < size.y && 
            0 <= pos.z && pos.z < size.z;
        public bool IsBlock(Vector3Int pos)
        {
            if (!InArea(pos) || !blocks.ContainsKey(pos) || !blockModels.ContainsKey(pos)) return false;
            return blocks.ContainsKey(pos) && blockModels.ContainsKey(pos);
        }
        public bool SetBlock(Vector3Int pos, BlockModel block)
        {
            if (!InArea(pos)) return false;
            DestroyBlock(pos);
            AddBlock(pos, block);
            return true;
        }
        public bool AddBlock(Vector3Int pos, BlockModel block)
        {
            if (!InArea(pos) || IsBlock(pos)) return false;

            block.transform.SetParent(this.transform);
            block.transform.localPosition = pos + Vector3.one * 0.5f;

            block.gameObject.name = $"({pos.x}, {pos.y}, {pos.z}) [{(int)block.blockID}] {block.blockID}";

            block.pos = pos;

            if(!blocks.ContainsKey(pos))blocks.Add(pos, block.blockID);
            if(!blockModels.ContainsKey(pos))blockModels.Add(pos, block);

            return true;
        }
        public bool RemoveBlock(Vector3Int pos)
        {
            if (!IsBlock(pos) || !InArea(pos)) return false;

            blocks.Remove(pos);
            blockModels.Remove(pos);
            return true;
        }
        public BlockModel CreateBlock(Vector3Int pos, BlockID id)
        {
            if (IsBlock(pos) || !InArea(pos)) return null;

            BlockModel block = Instantiate(
                BlockDB.Load(id)).GetComponent<BlockModel>();
            AddBlock(pos, block);
            return block;
        }
        public bool DestroyBlock(Vector3Int pos)
        {
            if (!IsBlock(pos) || !InArea(pos)) return false;

            if (blockModels.ContainsKey(pos) && blockModels[pos] != null)
                DestroyImmediate(blockModels[pos].gameObject);
            RemoveBlock(pos);
            return true;
        }

        public Vector3 WorldToMapPosition(Vector3 vec)
        {
            Matrix4x4 mat = transform.ToMat().inverse;
            return mat.MultiplyPoint(vec);
        }
        public Vector3Int WorldToMapIndex(Vector3 vec)
            => WorldToMapPosition(vec).ToVector3Int();
        public Vector3 MapIndexToWorldPosition(Vector3Int vec)
        {
            Matrix4x4 mat = transform.ToMat();
            return mat.MultiplyPoint(vec + Vector3.one*0.5f);
        }
    }
}
