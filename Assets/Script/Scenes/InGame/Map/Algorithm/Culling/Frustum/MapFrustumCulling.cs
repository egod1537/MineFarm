using Minefarm.Map.Algorithm.Culling;
using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Minefarm.Entity.Actor.Player;
using Minefarm.InGame;
using Unity.VisualScripting;

namespace Minefarm.Map.Algorithm.Culling
{
    public class MapFrustumCulling
    {
        private MapCulling culling;
        private MapModel model { get => culling.model; }

        private MapOcclusionCulling occlusion;

        public Vector3Int size;

        private ReactiveProperty<Vector3Int> centerPosition = new();
        private FrustumRect centerRect { 
            get => new FrustumRect(
                centerPosition.Value - size,
                centerPosition.Value + size);
        }

        private Vector3Int lastCenterPosition;
        private FrustumRect lastCenterRect
        {
            get => new FrustumRect(
                lastCenterPosition - size,
                lastCenterPosition + size);
        }

        public MapFrustumCulling(MapCulling culling, MapOcclusionCulling occlusion)
        {
            this.culling = culling;
            this.occlusion = occlusion;

            lastCenterPosition = -Vector3Int.one * 1_000_000_000;

            centerPosition.Subscribe(pos =>
            {
                UpdateVisibleArea();
                lastCenterPosition = pos;
            });
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = model.transform.ToMat();
            Gizmos.color = Color.blue;
            {
                Gizmos.DrawWireCube(centerPosition.Value, 2*size);
            }
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.identity;
        }

        public void Update()
        {
            centerPosition.Value = GetCenterPostion();
        }

        public bool IsVisibleArea(Vector3Int pos)
            => centerRect.InPoint(pos);

        private Vector3Int GetCenterPostion()
        {
            PlayerModel player = GameManager.ins.player;
            Vector3 pos = player.transform.position;
            return model.WorldToMapIndex(pos);
        }

        private void DNC(FrustumRect rect, FrustumRect now, FrustumRect last)
        {
            if (!rect.Valid()) return;
            bool incNow = now.IsInclude(rect);
            bool incLast = last.IsInclude(rect);
            if (incNow && incLast) return;
            if (incNow || incLast)
            {
                for (int x = rect.top.x; x <= rect.bottom.x; x++)
                    for (int y = rect.top.y; y <= rect.bottom.y; y++)
                        for (int z = rect.top.z; z <= rect.bottom.z; z++)
                        {
                            Vector3Int pos = new Vector3Int(x, y, z);
                            if (!model.IsBlock(pos)) continue;
                            if (incNow)
                                model.blockModels[pos].visible.Value = true;
                            if (incLast)
                                model.blockModels[pos].visible.Value = false;
                        }
                return;
            }

            if (rect.top == rect.bottom) return;

            if(rect.IsIntersect(now) || rect.IsIntersect(last))
            {
                Vector3Int mid = (rect.top + rect.bottom) / 2;
                Vector3Int f1 = rect.top, f2 = mid,
                    b1 = mid + Vector3Int.one, b2 = rect.bottom;

                for (int i = 0; i < (1 << 3); i++)
                {
                    FrustumRect to = new FrustumRect(f1, f2);
                    if ((i & (1 << 0)) > 0) { to.top.x = b1.x; to.bottom.x = b2.x; }
                    if ((i & (1 << 1)) > 0) { to.top.y = b1.y; to.bottom.y = b2.y; }
                    if ((i & (1 << 2)) > 0) { to.top.z = b1.z; to.bottom.z = b2.z; }

                    DNC(to, now, last);
                }
            }
        }

        private void UpdateVisibleArea()
        {
            DNC(new FrustumRect(Vector3Int.zero, model.size-Vector3Int.one),
                centerRect,
                lastCenterRect);
        }
    }
}
