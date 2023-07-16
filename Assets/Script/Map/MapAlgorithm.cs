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

        static readonly int[] dx = {1,-1,0,0,0,0 };
        static readonly int[] dy = {0,0,1,-1,0,0 };
        static readonly int[] dz = {0,0,0,0,1,-1 };

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
            if (!map.InArea(start) || !map.InArea(end)) return null;
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
                    !map.blocks.ContainsKey(pos + Vector3Int.up) || !map.InArea(pos + Vector3Int.up);
                return y1 && y2 && y3;
            }
            if(!chk(start) || !chk(end)) return null;

            PriorityQueue<Q> pq = new PriorityQueue<Q>();
            pq.Add(new Q(start, 0, end));
            set(start, 0);
            while(pq.Count > 0)
            {
                Q top = pq.Dequeue();
                if (get(top.pos) < top.cost) continue;
                for(int i=0; i < 6; i++)
                {
                    Vector3Int to = top.pos + new Vector3Int(dx[i], dy[i], dz[i]);
                    if (!chk(to)) continue;
                    int cot = top.cost + 1;
                    if(get(to) > cot)
                    {
                        set(to, cot);
                        pq.Enqueue(new Q(to, cot, end));

                        if (!track.ContainsKey(to)) track.Add(to, top.pos);
                        else track[to] = top.pos;
                    }
                }
            }

            List<Vector3Int> ret = new();
            ret.Add(end);
            while (end != start)
            {
                end = track[end];
                ret.Add(end);
            }
            ret.Reverse();
            return ret;
        }
    }
}
