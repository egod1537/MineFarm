using Minefarm.Entity.Actor.Block;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Map.Generator.WaveFunctionCollapse
{
    public class Bukkit
    {
        public int sz;
        public BlockID[,,] blocks;
        public Bukkit(int sz)
        {
            this.sz = sz;
            blocks = new BlockID[sz, sz,sz];
        }
        public override bool Equals(object obj)
        {
            Bukkit rhs = obj as Bukkit;
            if (sz != rhs.sz) return false;
            for (int x = 0; x < sz; x++)
                for (int y = 0; y < sz; y++)
                    for(int z=0; z < sz; z++)
                    if (rhs[x,y,z] != blocks[x,y,z])
                        return false;
            return true;
        }
        public bool IsEmpty()
        {
            for (int i = 0; i < sz; i++)
                for (int j = 0; j < sz; j++)
                    for (int k = 0; k < sz; k++)
                        if (blocks[i, j, k] != BlockID.Air)
                            return false;
            return true;
        }
        public BlockID this[int x, int y, int z]
        {
            get => blocks[x, y, z];
            set => blocks[x, y, z] = value;
        }
        public override int GetHashCode()
        {
            return 0;
        }
    }

    public class WaveFunctionCollapseMapGenerator : MapGenerator
    {
        public Vector3Int size;
        public int kernel;
        public List<MapModel> sampleModels;
        public int iteration;

        //Sampling Data
        private Dictionary<int, Bukkit> bukkits;
        private WeightedTable<int> bukkitCount;
        private WeightedTable<int> bukkitDistribution;
        private WeightedTable<Vector3Int> posDistribution;
        private AdjustTable adjustDistribution;

        //Processing Data
        private Dictionary<Vector3Int, WeightedTable<int>> percent;
        private Dictionary<Vector3Int, bool> visit;

        private Dictionary<Vector3Int, float> entropy;

        //Processing Result
        private Dictionary<Vector3Int, int> ret;

        const int COUNT_DIR = 26;
        readonly int[] dx = {  0, 0, 1, 1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1, 0, 0, 1, 1, 1, 0,-1,-1,-1};
        readonly int[] dy = { -1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        readonly int[] dz = {  0, 1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1, 0, 1, 0, 1, 1, 0,-1,-1,-1, 0, 1 };
              
        private void ProcessSampling()
        {
            new MapEncoder(this).Encode(sampleModels,
                out bukkits,
                out adjustDistribution,
                out posDistribution,
                out bukkitCount);
            bukkitDistribution = bukkitCount.Clone();
            bukkitDistribution.Normalize();
        }
        private float CalculateEntropy(WeightedTable<int> table)
        {
            float ans = 0.0f;
            foreach (var pair in table)
                ans += Mathf.Exp(pair.Value);
            ans *= table.Count;
            return ans;
        }
        private void ProcessInitialize()
        {
            percent = new();
            visit = new();
            ret = new();

            entropy = new();
            float startEntropy = CalculateEntropy(bukkitDistribution);

            for (int x = 0; x < size.x/kernel; x++)
                for (int y = 0; y < size.y/kernel; y++)
                    for (int z = 0; z < size.z/kernel; z++)
                    {
                        Vector3Int pos = new Vector3Int(x,y,z);
                        percent.Add(pos, bukkitDistribution.Clone());
                        visit.Add(pos, false);
                        ret.Add(pos, 0);

                        entropy.Add(pos, startEntropy);
                    }
        }
        public void Process()
        {
            size.x = size.x / kernel * kernel;
            size.y = size.y / kernel * kernel;
            size.z = size.z / kernel * kernel;
            ProcessSampling();

            for (int t = 0; t < iteration; t++)
            {
                ProcessInitialize();
                bool Run()
                {
                    while (true)
                    {
                        Vector3Int pos = GetPositionByRandom();
                        if (pos == -Vector3Int.one) break;
                        if (!DFS(pos)) return false;
                    }
                    return true;
                }
                if (Run())
                {
                    Debug.Log("Success");
                    return;
                }
            }
            Debug.Log("Failture");
        }
        private Vector3Int GetPositionByRandom()
        {
            WeightedTable<Vector3Int> table = new();
            float mx = 1000000000.0f;
            for(int x=0; x < size.x/kernel; x++)
                for(int y=0; y < size.y/kernel; y++)
                    for(int z=0; z < size.z/kernel; z++)
                    {
                        Vector3Int pos = new(x,y,z);
                        if (visit[pos]) continue;

                        float ent = entropy[pos];
                        if(Mathf.Approximately(ent, mx))
                            table.Put(pos);
                        else if(ent < mx)
                        {
                            mx = ent;
                            table = new();
                            table.Put(pos);
                        }
                    }
            if (table.Count == 0) return -Vector3Int.one;
            return table.Pick();
        }
        private bool Observe(Vector3Int pos)
        {
            int selected = 0;
            if (percent[pos].Count == 0)
                selected = bukkitDistribution.Pick();
            else
                selected = percent[pos].Pick();
            //if (percent[pos].Count == 0)
            //    return false;

            percent[pos].Clear();
            percent[pos].Put(selected);
            ret[pos] = selected;
            return true;
        }
        private void Propagate(Vector3Int pos)
        {
            for(int i=0; i < COUNT_DIR; i++)
            {
                Vector3Int to = pos + new Vector3Int(dx[i], dy[i], dz[i]);
                if (!InArea(to)) continue;
                AdjustPair pair = new(ret[pos], i);
                if (adjustDistribution.ContainsKey(pair))
                {
                    percent[to] *= adjustDistribution[pair];
                    percent[to].Normalize();

                    percent[to].Arrange();
                }
            }
        }

        private bool DFS(Vector3Int pos)
        {
            if (!Observe(pos)) return false;
            Propagate(pos);
            visit[pos] = true;
            for (int i = 0; i < COUNT_DIR; i++)
            {
                Vector3Int to = pos + new Vector3Int(dx[i], dy[i], dz[i]);
                if (!InArea(to) || visit[to]) continue;
                if (!DFS(to)) return false;
            }
            return true;
        }

        private bool InArea(Vector3Int pos)
        => 0 <= pos.x && pos.x < size.x/kernel
            && 0 <= pos.y && pos.y < size.y/kernel
            && 0 <= pos.z && pos.z < size.z/kernel;

        public override Dictionary<Vector3Int, BlockID> CreateLayout()
        {
            Process();
            return new MapDecoder(this).Decode(ret, bukkits);
        }
    }
}