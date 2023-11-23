using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UniRx;
using System.Text;

namespace Minefarm.Map.Algorithm.Culling
{
    public class MapOcclusionCulling
    {
        const int DEPTH = 5;

        private MapCulling culling;
        private MapModel model { get => culling.model; }

        private MapFrustumCulling frustum;
        private Camera mainCamera;
        public MapOcclusionCulling(MapCulling culling, MapFrustumCulling frustum)
        {
            this.culling = culling;
            this.frustum= frustum;

            this.mainCamera = Camera.main;

            model.onCreateBlock
                .Where(_ => Application.isPlaying)
                .Subscribe(pos => AddBlock(pos));
            model.onDestroyBlock
                .Where(_ => Application.isPlaying)
                .Subscribe(pos => RemoveBlock(pos));
        }

        public void AddBlock(Vector3Int pos)
            => Propagate(pos, true);
        public void RemoveBlock(Vector3Int pos)
            => Propagate(pos, false);

        private void Propagate(Vector3Int pos, bool mask)
        {
            TestCulling(pos);
            for (int i = 1; i <= DEPTH; i++)
            {
                for (int y = -i - 2; y <= -i + 1; y++)
                {
                    Vector3Int forward = pos + new Vector3Int(i, y, i);
                    Vector3Int left = pos + new Vector3Int(i, y, i - 1);
                    Vector3Int right = pos + new Vector3Int(i - 1, y, i);

                    if (CanPropagate(forward, mask)) TestCulling(forward);
                    if (CanPropagate(left, mask)) TestCulling(left);
                    if (CanPropagate(right, mask)) TestCulling(right);
                }
            }
        }

        private bool CanPropagate(Vector3Int pos, bool mask)
            => model.IsBlock(pos) && model.blockModels[pos].visible.Value == mask;

        private void TestCulling(Vector3Int pos)
        {
            if (!model.IsBlock(pos)) return;
            VisibleRange nowRange = GetFowardRange(model.blockModels[pos]);
            OcclusionUnionFind ufLeft = new(nowRange), ufRight = new(nowRange);

            bool IsCulling() => ufLeft.Check() && ufRight.Check();

            StringBuilder sb = new();
            sb.AppendLine($"Position : {pos} {nowRange.left} ~ {nowRange.right}");

            bool Test()
            {
                for (int i = 1; i <= DEPTH; i++)
                {
                    for (int y = i - 2; y <= i; y++)
                    {
                        Vector3Int forward = pos + new Vector3Int(-i, y, -i);

                        if (model.IsBlock(forward))
                        {
                            VisibleRange range = GetFowardRange(model.blockModels[forward]);
                            sb.AppendLine($"Forward : {forward} {range.left} ~ {range.right}");
                            ufLeft.Add(range);
                            ufRight.Add(range);
                        }
                        if (IsCulling()) return true;

                        Vector3Int left = pos + new Vector3Int(-i, y, -i + 1);
                        if (model.IsBlock(left))
                        {
                            VisibleRange range = GetSideRange(model.blockModels[left]);
                            sb.AppendLine($"Left : {left} {range.left} ~ {range.right}");
                            ufLeft.Add(range);
                        }
                        if (IsCulling()) return true;

                        Vector3Int right = pos + new Vector3Int(-i + 1, y, -i);
                        if (model.IsBlock(right))
                        {
                            VisibleRange range = GetSideRange(model.blockModels[right]);
                            sb.AppendLine($"Right : {right} {range.left} ~ {range.right}");
                            ufRight.Add(range);
                        }
                        if (IsCulling()) return true;
                    }
                }
                return false;
            }

            bool ret = Test();
            sb.AppendLine($"Ret : {ret}");
            Debug.Log(sb.ToString());
            culling.UpdateCulling(pos, ret);
        }

        private List<Vector3> ProjectionScreenAndCopy(List<Vector3> list)
        {
            List<Vector3> ret = new();
            foreach(Vector3 pos in list)
                ret.Add(mainCamera.WorldToScreenPoint(pos));
            return ret;
        }

        private VisibleRange GetFowardRange(BlockModel block)
        {
            List<Vector3> verts = ProjectionScreenAndCopy(block.vertices);
            verts = verts.OrderBy(v => v.y).ToList();
            return new(verts[0].y, verts[verts.Count - 1].y);
        }

        private VisibleRange GetSideRange(BlockModel block)
        {
            List<Vector3> verts = ProjectionScreenAndCopy(block.vertices);
            verts = verts.OrderBy(v => v.x).ToList();
            return new(verts[0].y, verts[1].y);
        }
    }
}

