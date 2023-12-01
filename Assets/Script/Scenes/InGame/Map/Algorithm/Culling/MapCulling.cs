using Minefarm.Entity.Actor.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditorInternal;
using UnityEngine;

namespace Minefarm.Map.Algorithm.Culling
{
    [Serializable]
    public class MapCulling 
    {
        [SerializeField]
        public MapModel model;

        public ReactiveProperty<bool> activeOcclusion = new();
        public ReactiveProperty<bool> activeFrustum = new();

        [SerializeField]
        public MapOcclusionCulling occlusion;
        [SerializeField]
        public MapFrustumCulling frustum;

        public MapCulling(MapModel model)
        {
            this.model = model;

            this.occlusion = new(this, frustum);
            this.frustum = new(this, occlusion);

            activeOcclusion.Value = activeOcclusion.Value;
            activeFrustum.Value = activeFrustum.Value;

            activeFrustum.Subscribe(_ =>
            {
                InitializeVisible();
            });
        }

        public void Update()
        {
            if (!Application.isPlaying) return;
            if(activeFrustum.Value) frustum.Update();
        }

        public void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            if (activeFrustum.Value) frustum.OnDrawGizmosSelected();
        }

        public void UpdateCulling(Vector3Int pos, bool culling)
        {
            if (!model.IsBlock(pos)) return;
            BlockModel block = model.blockModels[pos];
            if (activeFrustum.Value)
            {
                if (!frustum.IsVisibleArea(pos))
                    block.visible.Value = false;
                else
                    block.visible.Value = !culling;
            }
            else block.visible.Value = !culling;
        }

        public void InitializeVisible()
        {
            Vector3Int sz = model.size;
            for(int x=0; x < sz.x; x++)
                for(int y=0; y < sz.y; y++)
                    for(int z=0; z < sz.z; z++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        if (model.IsBlock(pos)) 
                            model.blockModels[pos].visible.Value = false;
                    }
        }
    }
}