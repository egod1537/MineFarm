using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    public class MapSPFADecoder
    {        
        //n = spfa.resolution
        //��ǥ�踦 n�� Ȯ���� �������� A* �˰����� �����Ѵ�.
        //n�� Ȯ���� �� ���� ��θ� ������ ��ǥ ��η� ��ȯ�Ѵ�.
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