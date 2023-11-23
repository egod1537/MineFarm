using Minefarm.Map.Algorithm.Culling;
using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.Culling
{
    public class MapFrustumCulling
    {
        private MapCulling culling;
        private MapModel model { get => model; }

        private MapOcclusionCulling occlusion;
        public MapFrustumCulling(MapCulling culling, MapOcclusionCulling occlusion)
        {
            this.culling = culling;
            this.occlusion = occlusion;
        }

        public bool IsVisibleArea(Vector3Int pos)
        {
            return true;
        }
    }
}
