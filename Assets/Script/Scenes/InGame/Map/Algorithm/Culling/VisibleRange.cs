using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Map.Algorithm.Culling
{
    public struct VisibleRange
    {
        const float EPS = 1e-3f;

        public float left, right;
        public VisibleRange(float left, float right)
        {
            this.left = left;
            this.right = right;

            if (this.left > this.right)
                (this.left, this.right) = (this.right, this.left);
        }
        public bool In(float x)
            => left - EPS <= x && x <= right + EPS;
        public bool Intersect(VisibleRange rhs)
            => rhs.In(left) || rhs.In(right);
        public bool Include(VisibleRange rhs)
            => left - EPS <= rhs.left && rhs.right <= right + EPS;
        public VisibleRange Merge(VisibleRange rhs)
            => new VisibleRange(Mathf.Min(left, rhs.left), Mathf.Max(right, rhs.right));
    }
}
