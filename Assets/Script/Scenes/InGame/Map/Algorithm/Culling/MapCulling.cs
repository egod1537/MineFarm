using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Minefarm.Map.Algorithm.Culling
{
    public class MapCulling 
    {
        public MapModel model;

        private MapOcclusionCulling occlusion;
        private MapFrustumCulling frustum;

        public MapCulling(MapModel model)
        {
            this.model = model;

            this.occlusion = new(this, frustum);
            this.frustum = new(this, occlusion);
        }

        public void UpdateCulling(Vector3Int pos, bool culling)
        {
            if (!model.IsBlock(pos)) return;
            BlockModel block = model.blockModels[pos];
            if (!frustum.IsVisibleArea(pos))
                block.visible.Value = false;
            else
                block.visible.Value = !culling;
        }
    }
}