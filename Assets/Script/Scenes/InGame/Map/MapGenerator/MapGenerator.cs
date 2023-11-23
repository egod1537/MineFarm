using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Map.Generator
{
    public abstract class MapGenerator : MonoBehaviour
    {
        public Vector3Int size;
        public MapModel target;
        public void Generate()
        {
            target.size = size;
            target.DestroyAllBlock();
            var data = CreateLayout();
            foreach (var pair in data)
                if(pair.Value != BlockID.Air)
                    target.CreateBlock(pair.Key, pair.Value);
            target.Save();
        }

        public void DestroyAll()
        {
            target.DestroyAllBlock();
            target.Save();
        }

        public abstract Dictionary<Vector3Int, BlockID> CreateLayout();
    }
}