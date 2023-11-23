using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Map.Generator.WaveFunctionCollapse
{
    public class MapEncoder
    {
        private WaveFunctionCollapseMapGenerator generator;
        private AdjustTable adjustTable;

        private int idxBukkit;
        private Dictionary<int, Bukkit> bukkits;
        private Dictionary<Bukkit, int> rbuc;
        private WeightedTable<Vector3Int> posDistribution;
        private WeightedTable<int> bukkitCount;

        private Vector3Int size { get => generator.size; }
        private int kernel { get => generator.kernel; }

        const int COUNT_DIR = 6;
        readonly int[] dx = { 1, -1, 0, 0, 0, 0 };
        readonly int[] dy = { 0, 0, 1, -1, 0, 0 };
        readonly int[] dz = { 0, 0, 0, 0, 1, -1 };

        public MapEncoder(WaveFunctionCollapseMapGenerator generator)
        {
            this.generator = generator;
        }

        private void Initialize()
        {
            idxBukkit = 0;
            bukkits = new();
            rbuc = new();

            bukkitCount = new();
            adjustTable = new();
            posDistribution = new();
        }

        private void IndexingBukkit(MapModel model)
        {
            Vector3Int sz = model.size;
            List<Bukkit> list = new();
            for (int x = 0; x < sz.x; x++)
                for (int y = 0; y <= sz.y-kernel; y++)
                    for (int z = 0; z < sz.z; z++)
                        list.Add(GetKernel(model, new Vector3Int(x, y, z)));

            foreach(var b in list)
            {
                if (!rbuc.ContainsKey(b))
                {
                    bukkits.Add(idxBukkit, b);
                    rbuc.Add(b, idxBukkit);
                    idxBukkit++;
                }

                int idx = rbuc[b];
                if (!bukkitCount.ContainsKey(idx))
                    bukkitCount.Add(idx, 0);
                bukkitCount[idx]++;
            }
        }
        private void IndexingBukkit(List<MapModel> models)
        {
            foreach (var model in models) IndexingBukkit(model);
        }

        private bool IsKernel(MapModel model, Vector3Int pos)
        {
            Vector3Int sz = model.size;
            return 0 <= pos.x && pos.x < sz.x
                && 0 <= pos.y && pos.y <= sz.y - kernel
                && 0 <= pos.z && pos.z < sz.z;
        }
        private Bukkit GetKernel(MapModel model, Vector3Int pivot)
        {
            Vector3Int sz = model.size;
            Bukkit ret = new(kernel);
            for(int x=0;x<kernel;x++)
                for(int y=0; y<kernel;y++)
                    for(int z=0; z<kernel;z++)
                    {
                        Vector3Int pos = pivot + new Vector3Int(x,y,z);
                        pos.x %= sz.x;
                        pos.z %= sz.z;
                        BlockID block = BlockID.Air;
                        if (model.blocks.ContainsKey(pos))
                            block = model.blocks[pos];
                        ret[x, y, z] = block;
                    }
            return ret;
        }

        private void Sampling(int px, int py, int pz, MapModel model)
        {
            Vector3Int sz = model.size;
            for(int x=px; x < sz.x;x += kernel)
                for(int y=py; y <= sz.y-kernel;y += kernel)
                    for(int z=pz; z < sz.z;z += kernel)
                    {
                        Vector3Int pos = new(x,y,z);
                        int now = rbuc[GetKernel(model, pos)];
                        for(int d=0; d < COUNT_DIR; d++)
                        {
                            Vector3Int to = pos + kernel*new Vector3Int(dx[d], dy[d], dz[d]);
                            if (!IsKernel(model, to)) continue;
                            adjustTable.Put(new AdjustPair(now, d), rbuc[GetKernel(model, to)]);
                        }
                    }
        }
        private void Sampling(MapModel model)
        {
            for (int x = 0; x < kernel; x++)
                for (int y = 0; y < kernel; y++)
                    for (int z = 0; z < kernel; z++)
                        Sampling(x,y,z,model);
        }

        public void Encode(List<MapModel> samples,
            out Dictionary<int, Bukkit> bukkits,
            out AdjustTable adjustTable,
            out WeightedTable<Vector3Int> posDistribution,
            out WeightedTable<int> bukkitCount)
        {
            Initialize();

            IndexingBukkit(samples);
            foreach (var model in samples)
                Sampling(model);

            this.adjustTable.Normalize();

            bukkits = new(this.bukkits);
            adjustTable = this.adjustTable.Clone();
            bukkitCount = this.bukkitCount.Clone();
            posDistribution = this.posDistribution.Clone();
        }
    }
}
