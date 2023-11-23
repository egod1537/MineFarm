using Minefarm.Entity.Actor.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    public class MapSPFAAlgorithm
    {
        private struct Astar : IComparable
        {
            public Vector3Int pos;
            public float cost;
            public float h;

            public Astar(Vector3Int pos, float cost, float h)
            {
                this.pos = pos;
                this.cost = cost;
                this.h = h;
            }

            public int CompareTo(object obj)
            {
                Astar rhs = (Astar)obj;
                if (cost+h < rhs.cost+rhs.h) return 1;
                else if (cost+h > rhs.cost+rhs.h) return -1;
                else return 0;
            }
        }

        private static MapSPFA spfa;
        private static Vector3Int start;
        private static Vector3Int destination;

        private static float ratioHeuristic;
        const float increaseRatioHeuristic = 0.05f;
        private static MapModel model {get => spfa.model;}
        private static int height;

        const float INF = 100_000_000.0f;
        const int COUNT_DIR = 24;
        static readonly int[] dx = {  0, 1, 1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1};
        static readonly int[] dy = { -1,-1,-1,-1,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
        static readonly int[] dz = {  1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1, 0, 1, 1, 1, 0,-1,-1,-1, 0, 1 };
        static readonly float[] COST = { 1.0f, 1.414f, 1.732f};

        private static float Cost(Vector3Int dir)
            => COST[Mathf.Abs(dir.x) + Mathf.Abs(dir.y) + Mathf.Abs(dir.z) - 1];
        private static float Cost(int dir)
            => Cost(new Vector3Int(dx[dir], dy[dir], dz[dir]));
        private static Vector3Int GetDirection(int dir)
        {
            Vector3Int ret = new Vector3Int(dx[dir], dy[dir], dz[dir]);
            ret.y *= spfa.resolution;
            return ret;
        }

        private static float heuristic(Vector3Int pos)
        {
            return ratioHeuristic * (Mathf.Abs(destination.x - pos.x)
                + Mathf.Abs(destination.y - pos.y)
                + Mathf.Abs(destination.z - pos.z));
        }


        //n = spfa.resolution
        //좌표계를 n배 확장한 공간에서 A* 알고리즘을 진행한다.
        public static List<Vector3Int> Run(
            MapSPFA spfa, 
            Vector3Int start, 
            Vector3Int destination,
            int height = 2)
        {
            MapSPFAAlgorithm.spfa = spfa;
            MapSPFAAlgorithm.height = height;
            MapSPFAAlgorithm.start = start;
            MapSPFAAlgorithm.destination = destination;

            Dictionary<Vector3Int, float> dst = new();
            void SetDistance(Vector3Int pos, float value)
            {
                if (!dst.ContainsKey(pos))
                    dst.Add(pos, INF);
                dst[pos] = value;
            }
            float GetDistance(Vector3Int pos)
            {
                if (!dst.ContainsKey(pos))
                    dst.Add(pos, INF);
                return dst[pos];
            }

            Dictionary<Vector3Int, Vector3Int> track = new();
            void SetTrack(Vector3Int pos, Vector3Int value)
            {
                if (!track.ContainsKey(pos))
                    track.Add(pos, Vector3Int.zero);
                track[pos] = value;
            }
            Vector3Int GetTrack(Vector3Int pos)
            {
                if (!track.ContainsKey(pos))
                    track.Add(pos, Vector3Int.zero);
                return track[pos];
            }

            PriorityQueue<Astar> pq = new();
            ratioHeuristic = 1.0f;

            //pumping y at start and destination 
            int n = spfa.resolution;
            while (start.y < model.size.y * n && !CanMove(start))
                start.y++;
            while (destination.y < model.size.y * n && !CanMove(destination))
                destination.y++;

            if (CanMove(start))
            {
                pq.Enqueue(new Astar(start, 0.0f, heuristic(start))); SetDistance(start, 0);
                while (pq.Count > 0)
                {
                    Astar top = pq.Dequeue();
                    if (top.pos == destination) break;
                    if (GetDistance(top.pos) < top.cost) continue;

                    for (int i = 0; i < COUNT_DIR; i++)
                    {
                        Vector3Int to = top.pos + GetDirection(i);
                        if (!CanMove(to) || !CanDirection(top.pos, i)) continue;
                        float toCost = top.cost + Cost(i);
                        if (GetDistance(to) > toCost)
                        {
                            SetDistance(to, toCost);
                            pq.Enqueue(new Astar(to, toCost, heuristic(to)));
                            SetTrack(to, top.pos);
                        }
                    }
                    ratioHeuristic += increaseRatioHeuristic;
                }
            }

            List<Vector3Int> ret = new();
                
            if(GetDistance(destination) != INF)
            {
                Vector3Int now = destination;
                while(now != start)
                {
                    ret.Add(now);
                    now = GetTrack(now);
                }
                ret.Add(start);
                ret.Reverse();
            }
            return ret;
        }

        private static bool InArea(Vector3Int pos)
        {
            int n = spfa.resolution;
            return 0 <= pos.x && pos.x < n * model.size.x
                && 0 <= pos.z && pos.z < n * model.size.z;
        }
        private static bool CanMove(Vector3Int pos, int height)
        {
            if (!InArea(pos)) return false;
            if (IsAir(pos + Vector3Int.down)) return false;
            for (int h = 0; h < height; h++)
                if (!IsAir(pos + Vector3Int.up * h)) return false;
            return true;
            return IsAir(pos) && IsAir(pos + Vector3Int.up) && !IsAir(pos + Vector3Int.down);
        }
        private static bool CanMove(Vector3Int pos)
            => CanMove(pos, MapSPFAAlgorithm.height);
        //대각선 이동 시 양 옆의 벽 존재여부
        private static bool CanDirection(Vector3Int pos, int dir)
        {
            Vector3Int to = GetDirection(dir);
            if(to.x != 0 && to.z != 0)
            {
                Vector3Int xPos = pos + new Vector3Int(to.x, to.y, 0);
                Vector3Int zPos = pos + new Vector3Int(0, to.y, to.z);

                bool ret = CanMove(xPos) && CanMove(zPos);
                if (to.y > 0)
                    ret |= CanMove(xPos + Vector3Int.down, height + 1) &&
                        CanMove(zPos + Vector3Int.down, height + 1);
                else if (to.y < 0)
                    ret |= CanMove(pos + to, height + 1) &&
                        CanMove(xPos + Vector3Int.up) &&
                        CanMove(zPos + Vector3Int.up);

                return ret;
            }
            return true;
        }
        private static bool IsAir(Vector3Int pos)
            => model.IsAir(NPosToMapPos(pos));
        private static Vector3Int NPosToMapPos(Vector3Int pos)
            => pos / spfa.resolution;
    }
}
