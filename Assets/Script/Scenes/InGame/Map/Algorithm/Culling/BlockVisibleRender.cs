using Minefarm.Entity.Actor.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
namespace Minefarm.Map.Algorithm.Culling
{
    public class BlockVisibleRender
    {
        private BlockModel model;
        private Transform body { get => model.body; }

        public ReactiveProperty<bool> visible = new();
        public VisibleRange fowardRange;
        public VisibleRange sideRange;

        public BlockVisibleRender(BlockModel model)
        {
            this.model = model;
            visible.Value = true;

            AddReactiveVisible();

            if (!Application.isPlaying) return;
            PreprocessingRanges();
        }

        private void AddReactiveVisible()
        {
            visible.Subscribe(v => body.gameObject.SetActive(v));
        }
        private void PreprocessingRanges()
        {
            List<Vector3> verts = body.GetComponent<MeshFilter>().mesh.vertices.ToList();
            for(int i=0; i <verts.Count; i++)
            {
                verts[i] = body.localToWorldMatrix.MultiplyPoint3x4(verts[i]);
                verts[i] = Camera.main.WorldToScreenPoint(verts[i]);
            }

            PreprocessingFowardRange(verts);
            PreprocessingSideRange(verts);
        }
        private void PreprocessingFowardRange(List<Vector3> verts)
        {
            verts = verts.OrderBy(v => v.y).ToList();
            fowardRange = new(verts[0].y, verts[verts.Count-1].y);
        }
        private void PreprocessingSideRange(List<Vector3> verts)
        {
            verts = verts.OrderBy(v => v.x).ToList();
            sideRange = new(verts[0].y, verts[1].y);
        }
    }
}