using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Minefarm.Map.Algorithm.Culling
{
    public struct FrustumRect
    {
        public Vector3Int top, bottom;
        public FrustumRect(Vector3Int top, Vector3Int bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }
        public bool Valid()
            => top.x <= bottom.x  && top.y <= bottom.y && top.z <= bottom.z;
        public bool IsIntersect(FrustumRect other)
        {
            if (other.bottom.x < top.x || bottom.x < other.top.x) return false;
            if (other.bottom.y < top.y || bottom.y < other.top.y) return false;
            if (other.bottom.z < top.z || bottom.z < other.top.z) return false;
            return true;
        }
        public bool IsInclude(FrustumRect other)
        {
            return top.x <= other.top.x && other.bottom.x <= bottom.x
                && top.y <= other.top.y && other.bottom.y <= bottom.y
                && top.z <= other.top.z && other.bottom.z <= bottom.z;
        }

        public bool InPoint(Vector3Int point)
         => top.x <= point.x && point.x <= bottom.x
            && top.y <= point.y && point.y <= bottom.y
            && top.z <= point.z && point.z <= bottom.z;
        public List<Vector3Int> GetPoints()
            => new List<Vector3Int>() {
            top,
            new Vector3Int(top.x, top.y, bottom.z),
            new Vector3Int(bottom.x, top.y, bottom.z),
            new Vector3Int(bottom.x, top.y, top.z),
            new Vector3Int(top.x, bottom.y, top.z),
            new Vector3Int(top.x, bottom.y, bottom.z),
            bottom,
            new Vector3Int(bottom.x, bottom.y, top.z),};
    }
}

