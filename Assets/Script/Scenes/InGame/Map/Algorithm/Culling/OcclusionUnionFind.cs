using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.Culling
{
    public class OcclusionUnionFind
    {
        private List<int> dsu;

        private VisibleRange target;
        private List<VisibleRange> ranges;
        private bool ans;
        public OcclusionUnionFind(VisibleRange target)
        {
            dsu = new();

            this.target = target;
            ranges = new List<VisibleRange>();

            ans = false;
        }

        public void Add(VisibleRange range)
        {
            dsu.Add(dsu.Count);
            ranges.Add(range);
            UpdateAnswer(range);
            for (int i = 0; i < dsu.Count - 1; i++)
            {
                VisibleRange to = ranges[Find(i)];
                if(range.Intersect(to) ||
                    range.Include(to) ||
                    to.Include(range))
                    Merge(i, dsu.Count - 1);
            }
        }
        public int Find(int x)
        {
            if (dsu[x] == x) return x;
            int ret = Find(dsu[x]);
            ranges[ret] = ranges[ret].Merge(ranges[x]);
            UpdateAnswer(ranges[ret]);
            return dsu[x] = ret;
        }
        public void Merge(int a, int b)
        {
            a = Find(a); b = Find(b);
            if (a == b) return;
            dsu[b] = a;
            ranges[a] = ranges[a].Merge(ranges[b]);
            UpdateAnswer(ranges[a]);
        }

        public void UpdateAnswer(VisibleRange range)
        {
            if (range.Include(target)) 
                ans = true;
        }
        public bool Check() => ans;
    }
}

