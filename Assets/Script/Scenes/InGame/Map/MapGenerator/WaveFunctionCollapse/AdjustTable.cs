using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Map.Generator.WaveFunctionCollapse
{
    public struct AdjustPair
    {
        public int block;
        public int dir;
        public AdjustPair(int block, int dir)
        {
            this.block = block;
            this.dir = dir;
        }
        public override int GetHashCode()
        {
            return block.GetHashCode() ^ dir.GetHashCode();
        }
    }
    public class AdjustTable : Dictionary<AdjustPair, WeightedTable<int>>
    {

        public void Put(AdjustPair pair, int block, int p = 1)
        {
            if (!this.ContainsKey(pair))
                base.Add(pair, new());
            base[pair].Put(block, p);
        }
        public void Put(int center, int dir, int block, int p = 1)
            => Put(new AdjustPair(center, dir), block, p);
        public void Normalize()
        {
            foreach (var pair in this)
                pair.Value.Normalize();
        }
        public AdjustTable Clone()
        {
            AdjustTable ret = new();
            foreach (var pair in this)
                ret.Add(pair.Key, pair.Value.Clone());
            return ret;
        }
    }
}