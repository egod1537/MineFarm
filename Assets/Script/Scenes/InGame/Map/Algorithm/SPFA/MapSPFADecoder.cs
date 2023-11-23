using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    public class MapSPFADecoder
    {        
        //n = spfa.resolution
        //좌표계를 n배 확장한 공간에서 A* 알고리즘을 진행한다.
        //n배 확장한 맵 상의 경로를 월드의 좌표 경로로 변환한다.
        public static List<Vector3> Decode(
            MapSPFA spfa, 
            List<Vector3Int> route)
        {
            MapModel model = spfa.model;
            Matrix4x4 trMatInv = model.transform.ToMat().inverse;

            int n = spfa.resolution;
            List<Vector3> ret = new();
            foreach(var value in route)
            {
                Vector3 pos = value;
                pos /= n;
                ret.Add(trMatInv.MultiplyPoint(pos));
            }
            return ret;
        }
    }
}