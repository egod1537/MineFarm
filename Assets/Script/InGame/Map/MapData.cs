using Minefarm.Entity.Actor.Block;
using Minefarm.Map.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Map
{
    [Serializable]
    public class MapData
    {
        [SerializeField]
        public Vector3Int size;

        [Serializable]
        public class BlockDictionary : SerializableDictionary<Vector3Int, BlockID> { }
        [SerializeField]
        public BlockDictionary blocks;

        public MapData()
        {
            size = Vector3Int.zero;
            blocks = new BlockDictionary(); 
        }
    }
}