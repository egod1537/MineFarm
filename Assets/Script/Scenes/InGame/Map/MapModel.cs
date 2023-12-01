using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Unity.VisualScripting;
using static Minefarm.Map.MapData;
using Minefarm.Entity.Actor.Block;
using System.Linq;
using Minefarm.Map.Algorithm.SPFA;
using Minefarm.Map.Algorithm.Culling;

namespace Minefarm.Map
{
    [Serializable]
    [ExecuteAlways]
    [RequireComponent(typeof(BoxCollider))]
    public class MapModel : MonoBehaviour
    {
        string PATH_MAPDATA { get => Application.dataPath + "/Resources/MapData"; }

        public Subject<Vector3Int> onCreateBlock = new();
        public Subject<Vector3Int> onDestroyBlock = new();
        public Subject<Vector3Int> onResize = new();

        public string mapDataPath;
        public MapData mapData;

        public Vector3Int size
        {
            get => mapData.size;
            set => Resize(value);
        }

        [SerializeField]
        public MapSPFA pathFinder = new();

        [SerializeField]
        public BlockDictionary blocks { 
            get => mapData.blocks; 
            set => mapData.blocks = value;
        }

        public Dictionary<Vector3Int, BlockModel> blockModels
            = new Dictionary<Vector3Int, BlockModel>();

        private BoxCollider _boxCollider;
        public BoxCollider boxCollider { get => GetComponent<BoxCollider>(); }

        [SerializeField]
        public MapCulling culling;
        public void Awake()
        {
            culling = new(this);
        }

        public void OnEnable()
        {
            Load();
            pathFinder.model = this;
        }

        public virtual void Update()
        {
            culling.Update();
        }

        public void OnDrawGizmosSelected()
        {
            pathFinder.OnDrawGizmosSelected();
            culling.OnDrawGizmosSelected();
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
            try
            {
                mapData = JsonDB.LoadJsonFile<MapData>($"{PATH_MAPDATA}/{mapDataPath}");
            }catch(Exception e)
            {
                Save();
                mapData = JsonDB.LoadJsonFile<MapData>($"{PATH_MAPDATA}/{mapDataPath}");
            }
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
        public bool IsAir(Vector3Int pos)
        {
            if (!IsBlock(pos)) return true;
            return blocks[pos] == BlockID.Air;
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

            block.Initialize();

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
                BlockDB.LoadBlock(id)).GetComponent<BlockModel>();
            AddBlock(pos, block);
            onCreateBlock.OnNext(pos);
            return block;
        }
        public bool DestroyBlock(Vector3Int pos)
        {
            if (!IsBlock(pos) || !InArea(pos)) return false;

            if (blockModels.ContainsKey(pos) && blockModels[pos] != null)
                DestroyImmediate(blockModels[pos].gameObject);
            RemoveBlock(pos);
            onDestroyBlock.OnNext(pos);
            return true;
        }
        public void DestroyAllBlock()
        {
            List<Vector3Int> poses = blockModels.Keys.ToList();
            foreach (var pos in poses)
                DestroyBlock(pos);
        }

        public const int COUNT_DIR = 6;
        public static readonly int[] dx = { 1, -1, 0, 0, 0, 0 };
        public static readonly int[] dy = { 0, 0, 1, -1, 0, 0 };
        public static readonly int[] dz = { 0, 0, 0, 0, 1, -1 };

        public List<BlockID> GetAdjustBlock(Vector3Int pos)
        {
            List<BlockID> ret = new List<BlockID>();
            for(int i=0; i < COUNT_DIR; i++)
            {
                Vector3Int to = pos + new Vector3Int(dx[i], dy[i], dz[i]);
                if (!InArea(to)) ret.Add(BlockID.Air);
                else ret.Add(blocks[to]);
            }
            return ret;
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
