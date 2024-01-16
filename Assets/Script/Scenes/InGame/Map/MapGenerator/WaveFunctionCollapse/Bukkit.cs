using Minefarm.Entity.Actor.Block;
using System.Collections;
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
            blocks = new BlockID[sz, sz, sz];
        }
        public override bool Equals(object obj)
        {
            Bukkit rhs = obj as Bukkit;
            if (sz != rhs.sz) return false;
            for (int x = 0; x < sz; x++)
                for (int y = 0; y < sz; y++)
                    for (int z = 0; z < sz; z++)
                        if (rhs[x, y, z] != blocks[x, y, z])
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

}