using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Map.Algorithm.SPFA
{
    [Serializable]
    public class MapSPFAViewer
    {
        public bool isMapField;
        public bool isMapWall;
        public bool isMapRoute;

        [SerializeField]
        private MapSPFA spfa;
        private MapModel model { get => spfa.model; }

        public MapSPFAViewer(MapSPFA spfa)
        {
            this.spfa = spfa;
        }

        public void OnDrawGizmosSelected()
        {
            if (isMapField) DrawGrid();
            if (isMapWall) DrawWalls();
            if (isMapRoute) DrawRoute();
        }

        private void DrawGrid()
        {
            Gizmos.matrix = model.transform.ToMat();
            Gizmos.color = Color.red;
            {
                void Draw(Vector3Int pivot)
                {
                    int n = spfa.resolution;
                    IterateSperatedSpace(pivot, pos =>
                    {
                        Vector3 size = Vector3.one / n;
                        Gizmos.DrawWireCube(pos, size);
                    });
                }

                IterateModelSpace(pos => Draw(pos));
            }
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.identity;
        }
        private void DrawWalls()
        {
            Gizmos.matrix = model.transform.ToMat();
            Gizmos.color = Color.gray.SetOpacity(0.5f);
            {
                void Draw(Vector3Int pivot)
                {
                    int n = spfa.resolution;
                    IterateSperatedSpace(pivot, pos =>
                    {
                        Vector3 size = Vector3.one / n;
                        Gizmos.DrawCube(pos, size);
                    });
                }
                IterateModelSpace(pos =>
                {
                    if (!model.IsBlock(pos)) return;
                    Draw(pos);
                });
            }
            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.identity;
        }
        private void DrawRoute()
        {
            Gizmos.matrix = model.transform.ToMat();
            Gizmos.color = Color.blue;
            {
                int n = spfa.resolution;
                foreach (var pos in spfa.cache)
                {
                    Gizmos.DrawCube(pos + Vector3.one / (2 * n), Vector3.one / n);
                }
            }
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.identity;

        }

        private void IterateModelSpace(UnityAction<Vector3Int> callback)
        {
            for (int i = 0; i < model.size.x; i++)
                for (int j = 0; j < model.size.y; j++)
                    for (int k = 0; k < model.size.z; k++)
                        callback(new Vector3Int(i,j,k));
        }

        private void IterateSperatedSpace(Vector3Int pos, UnityAction<Vector3> callback)
        {
            int n = spfa.resolution;
            for (int a = 0; a < n; a++)
                for (int b = 0; b < n; b++)
                    for (int c = 0; c < n; c++)
                    {
                        Vector3 now = pos + new Vector3(a, b, c) / n;
                        Vector3 pivot = Vector3.one / (2 * n);
                        callback(now+pivot);
                    }
        }
    }
}
