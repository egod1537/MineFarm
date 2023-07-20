using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Block
{
    public static class ExBlockID 
    {
        public static bool IsPassable(this BlockID id)
        {
            return false;
        }

        public static bool IsBrakable(this BlockID id)
        {
            return false;
        }
    }
}