using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;

namespace Minefarm.Map
{
    public class MapAlgorithm : MonoBehaviour
    {
        const int INF = 1_000_000_000;

        static readonly int[] dx = {1,-1,0,0 };
        static readonly int[] dz = {0,0,1,-1};

        public struct Q : IComparable
        {
            public Vector3Int pos;
            public int cost;
            public Vector3Int end;

            public Q(Vector3Int pos, int cost, Vector3Int end)
            {
                this.pos = pos;
                this.cost = cost;
                this.end = end;
            }
            public int CompareTo(object obj)
            {
                Q rhs = (Q)obj;
                int da = cost + (pos - end).SumAbs(), db = rhs.cost + (rhs.pos - rhs.end).SumAbs();
                return da - db;
            }
        }

        /// <summary>
        /// Shortest Path using Dijkstra
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Vector3Int> ShortestPath(MapModel map, Vector3Int start, Vector3Int end)
        {
            List<Vector3Int> ret = new();
            if (!map.InArea(start) || !map.InArea(end)) return ret;
            Dictionary<Vector3Int, int> dst = new();
            Dictionary<Vector3Int, Vector3Int> track = new();

            int get(Vector3Int pos)
            {
                if (!dst.ContainsKey(pos)) dst.Add(pos, INF);
                return dst[pos];
            }
            void set(Vector3Int pos, int cost)
            {
                if (!dst.ContainsKey(pos)) dst.Add(pos, cost);
                else dst[pos] = cost;
            }
            bool chk(Vector3Int pos)
            {
                bool y1 =
                    map.InArea(pos - Vector3Int.up) && map.blocks.ContainsKey(pos - Vector3Int.up);
                bool y2 =
                    map.InArea(pos) && !map.blocks.ContainsKey(pos);
                bool y3 =
                    !map.InArea(pos + Vector3Int.up) || !map.blocks.ContainsKey(pos + Vector3Int.up);
                return y1 && y2 && y3;
            }
            
            if(!chk(start) || !chk(end)) return ret;

            PriorityQueue<Q> pq = new PriorityQueue<Q>();
            pq.Add(new Q(start, 0, end));
            set(start, 0);
            bool isArrived = false;
            while(pq.Count > 0)
            {
                Q top = pq.Dequeue();
                if (top.pos == end) isArrived = true;
                if (get(top.pos) < top.cost) continue;

                for(int dy=-1; dy <= 1; dy++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3Int to = top.pos + new Vector3Int(dx[i], dy, dz[i]);
                        if (!chk(to)) continue;
                        int cot = top.cost + 1 + Mathf.Abs(dy);
                        if (get(to) > cot)
                        {
                            set(to, cot);
                            pq.Enqueue(new Q(to, cot, end));

                            if (!track.ContainsKey(to)) track.Add(to, top.pos);
                            else track[to] = top.pos;
                        }
                    }
                }

                for(int i=0; i < 2; i++)
                    for(int j=2; j < 4; j++)
                    {
                        bool ok = true;
                        for (int a = 0; a < 2; a++)
                            for (int b = 0; b < 2; b++)
                                if (!chk(top.pos + new Vector3Int(a * dx[i], 0, b * dz[j])))
                                    ok = false;
                        if (!ok) continue;
                        Vector3Int to = top.pos + new Vector3Int(dx[i], 0, dz[j]);
                        int cot = top.cost + 2;
                        if (get(to) > cot)
                        {
                            set(to, cot);
                            pq.Enqueue(new Q(to, cot, end));

                            if (!track.ContainsKey(to)) track.Add(to, top.pos);
                            else track[to] = top.pos;
                        }
                    }
            }
            if (!isArrived) return ret;

            ret.Add(end);
            while (end != start)
            {
                end = track[end];
                ret.Add(end);
            }
            ret.Reverse();
            return ret;
        }
        public static void DrawPath(MapModel model, List<Vector3Int> route)
        {
            if (route == null) return;
            Gizmos.matrix = model.transform.ToMat();
            Color color = Color.blue;
            color.a = 0.5f;
            Gizmos.color = color;

            foreach(var pos in route)
            {
                Vector3 worldPos = pos + Vector3.one * 0.5f;
                Gizmos.DrawCube(worldPos, Vector3.one);
            }
        }
    }
}
