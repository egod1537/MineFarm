using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UniRx;
using System.Text;
using Unity.VisualScripting;
using System.Threading;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;

namespace Minefarm.Map.Algorithm.Culling
{
    public class MapOcclusionCulling
    {
        const int DEPTH = 5;

        private MapCulling culling;
        private MapModel model { get => culling.model; }

        private MapFrustumCulling frustum;
        private Camera mainCamera;

        private Dictionary<Vector3Int, Thread> threads = new();
        public MapOcclusionCulling(MapCulling culling, MapFrustumCulling frustum)
        {
            this.culling = culling;
            this.frustum= frustum;

            this.mainCamera = Camera.main;

            culling.activeOcclusion
                .Where(_ => Application.isPlaying)
                .Subscribe(v =>
                {
                    if (v) TurnOnCulling();
                    else TurnOffCulling();
                });

            model.onCreateBlock
                .Where(_ => culling.activeOcclusion.Value)
                .Where(_ => Application.isPlaying)
                .Subscribe(pos => AddBlock(pos));
            model.onDestroyBlock
                .Where(_ => culling.activeOcclusion.Value)
                .Where(_ => Application.isPlaying)
                .Subscribe(pos => RemoveBlock(pos));
        }

        public void TurnOnCulling()
        {
            Vector3Int sz = model.size;
            for (int i = 0; i < sz.x; i++)
                for (int j = 0; j < sz.y; j++)
                    for (int k = 0; k < sz.z; k++)
                        RemoveBlock(new Vector3Int(i,j,k));

            for (int i = 0; i < sz.x; i++)
                for (int j = 0; j < sz.y; j++)
                    for (int k = 0; k < sz.z; k++)
                        AddBlock(new Vector3Int(i, j, k));
        }
        public void TurnOffCulling()
        {
            Vector3Int sz = model.size;
            for (int i = 0; i < sz.x; i++)
                for (int j = 0; j < sz.y; j++)
                    for (int k = 0; k < sz.z; k++)
                        culling.UpdateCulling(new Vector3Int(i,j,k), false);
        }

        public void AddBlock(Vector3Int pos)
            => Propagate(pos, true);
        public void RemoveBlock(Vector3Int pos)
            => Propagate(pos, false);

        private void Propagate(Vector3Int pos, bool mask)
        {
            TestCullingUsingLineSweeper(pos);
            if (CanPropagate(pos + Vector3Int.down, mask))
                TestCullingUsingLineSweeper(pos + Vector3Int.down);

            for (int i = 1; i <= DEPTH; i++)
            {
                for (int y = -i - 2; y <= -i + 1; y++)
                {
                    Vector3Int forward = pos + new Vector3Int(i, y, i);
                    Vector3Int left = pos + new Vector3Int(i, y, i - 1);
                    Vector3Int right = pos + new Vector3Int(i - 1, y, i);

                    //if (CanPropagate(forward, mask)) 
                    //    TestCullingUsingLineSweeper(forward);
                    //if (CanPropagate(left, mask)) 
                    //    TestCullingUsingLineSweeper(left);
                    //if (CanPropagate(right, mask)) 
                    //    TestCullingUsingLineSweeper(right);

                    if (CanPropagate(forward, mask))
                        TestCullingUsingLineSweeper(forward);
                    if (CanPropagate(left, mask))
                        TestCullingUsingLineSweeper(left);
                    if (CanPropagate(right, mask))
                        TestCullingUsingLineSweeper(right);
                }
            }
        }

        private bool CanPropagate(Vector3Int pos, bool mask)
            => model.IsBlock(pos) && model.blockModels[pos].visible.Value == mask;

        //private int TestCullingUsingDSU(Vector3Int pos)
        //{
        //    if (!model.IsBlock(pos)) return - 1;
        //    VisibleRange nowRange = GetForwardRange(model.blockModels[pos]);
        //    OcclusionUnionFind ufLeft = new(nowRange), ufRight = new(nowRange);

        //    bool IsCulling() => ufLeft.Check() && ufRight.Check();

        //    Vector3Int upPos = pos + Vector3Int.up;
        //    if (model.IsBlock(upPos))
        //    {
        //        VisibleRange range = GetForwardRange(model.blockModels[upPos]);
        //        ufLeft.Add(range);
        //        ufRight.Add(range);
        //    }

        //    bool Test()
        //    {
        //        for (int i = 1; i <= DEPTH; i++)
        //        {
        //            for (int y = i - 2; y <= i; y++)
        //            {
        //                Vector3Int forward = pos + new Vector3Int(-i, y, -i);

        //                if (model.IsBlock(forward))
        //                {
        //                    VisibleRange range = GetFowardRange(model.blockModels[forward]);
        //                    ufLeft.Add(range);
        //                    ufRight.Add(range);
        //                }
        //                if (IsCulling()) return true;

        //                Vector3Int left = pos + new Vector3Int(-i, y, -i + 1);
        //                if (model.IsBlock(left))
        //                {
        //                    VisibleRange range = GetSideRange(model.blockModels[left]);
        //                    ufLeft.Add(range);
        //                }
        //                if (IsCulling()) return true;

        //                Vector3Int right = pos + new Vector3Int(-i + 1, y, -i);
        //                if (model.IsBlock(right))
        //                {
        //                    VisibleRange range = GetSideRange(model.blockModels[right]);
        //                    ufRight.Add(range);
        //                }
        //                if (IsCulling()) return true;
        //            }
        //        }
        //        return false;
        //    }

        //    return (Test() ? 1 : 0);
        //}

        private void TestCullingUsingLineSweeper(Vector3Int pos)
        {
            if (!model.IsBlock(pos)) return;
            VisibleRange nowRange = GetForwardRange(model.blockModels[pos]);
            OcclusionLineSweeper leftSweeper = new(nowRange), rightSweeper = new(nowRange);

            bool IsCulling() => leftSweeper.Check() && rightSweeper.Check();

            Vector3Int upPos = pos + Vector3Int.up;
            bool isUp = false;
            if (model.IsBlock(upPos))
            {
                VisibleRange range = GetSideRange(model.blockModels[upPos]);
                leftSweeper.Add(range);
                rightSweeper.Add(range);
                isUp = true;
            }

            bool Test()
            {
                for (int i = 1; i <= DEPTH; i++)
                {
                    for (int y = i - 2; y <= i; y++)
                    {
                        Vector3Int forward = pos + new Vector3Int(-i, y, -i);

                        if (model.IsBlock(forward))
                        {
                            VisibleRange range = GetForwardRange(model.blockModels[forward]);
                            if (isUp && y <= pos.y) range = GetSideRange(model.blockModels[forward]);
                            leftSweeper.Add(range);
                            rightSweeper.Add(range);
                        }

                        Vector3Int left = pos + new Vector3Int(-i, y, -i + 1);
                        if (model.IsBlock(left))
                        {
                            VisibleRange range = 
                                (isUp ? GetForwardRange(model.blockModels[left]) : GetSideRange(model.blockModels[left]));
                            leftSweeper.Add(range);
                        }

                        Vector3Int right = pos + new Vector3Int(-i + 1, y, -i);
                        if (model.IsBlock(right))
                        {
                            VisibleRange range =
                                (isUp ? GetForwardRange(model.blockModels[right]) : GetSideRange(model.blockModels[right]));
                            rightSweeper.Add(range);
                        }
                    }

                    if (IsCulling()) return true;
                }
                return false;
            }

            culling.UpdateCulling(pos, Test());
        }

        private VisibleRange GetForwardRange(BlockModel block)
        {
            List<Vector3> verts = new(block.vertices);
            for(int i=0; i < verts.Count; i++)
                verts[i] = Camera.main.WorldToScreenPoint(block.vertices[i]);
            verts = verts.OrderBy(v => v.y).ToList();
            return new(verts[0].y, verts[verts.Count - 1].y);
        }

        private VisibleRange GetSideRange(BlockModel block)
        {
            List<Vector3> verts = new(block.vertices);
            for (int i = 0; i < verts.Count; i++)
                verts[i] = Camera.main.WorldToScreenPoint(block.vertices[i]);
            verts = verts.OrderBy(v => v.x).ToList();
            return new(verts[0].y, verts[1].y);
        }
    }
}

