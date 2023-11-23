using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Minefarm.Map.Generator.WaveFunctionCollapse
{
    public class MapDecoder
    {
        private WaveFunctionCollapseMapGenerator generator;

        private int kernel { get => generator.kernel; }
        private Vector3Int size { get => generator.size; }
        public MapDecoder(WaveFunctionCollapseMapGenerator generator)
        {
            this.generator = generator;
        }

        public Dictionary<Vector3Int, BlockID> Decode(
            Dictionary<Vector3Int, int> map, Dictionary<int, Bukkit> bukkit)
        {
            Dictionary<Vector3Int, BlockID> ret = new();
            foreach(var pair in map)
            {
                Vector3Int p = kernel* pair.Key;
                Bukkit now = bukkit[pair.Value];
                for (int x = 0; x < kernel; x++)
                    for (int y = 0; y < kernel; y++)
                        for (int z = 0; z < kernel; z++)
                            ret.Add(p+new Vector3Int(x,y,z), now[x,y,z]);
            }
            return ret;
        }
    }

}
