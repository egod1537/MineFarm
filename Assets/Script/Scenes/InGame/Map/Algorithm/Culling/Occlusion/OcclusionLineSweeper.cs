using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Map.Algorithm.Culling
{
    public class OcclusionLineSweeper
    {
        public VisibleRange target;
        private List<VisibleRange> lines = new();
        public OcclusionLineSweeper(VisibleRange target)
        {
            this.target = target;
        }
        public bool Check()
        {
            Relax();
            foreach (var line in lines)
                if (line.Include(target)) return true;
            return false;   
        }
        public void Add(VisibleRange line)
            => lines.Add(line);

        public void Relax()
        {
            if (lines.Count == 0) return;
            lines = lines.OrderBy(v => v.left).ToList();

            List<VisibleRange> newLines = new();
            VisibleRange now = lines[0];
            foreach (VisibleRange line in lines)
                if (now.Intersect(line))
                    now.right = Mathf.Max(now.right, line.right);
                else
                {
                    newLines.Add(now);
                    now = line;
                }
            newLines.Add(now);

            lines = newLines;
        }
    }
}