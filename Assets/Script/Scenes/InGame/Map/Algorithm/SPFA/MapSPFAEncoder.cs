using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    public class MapSPFAEncoder
    {   
        //n = spfa.resolution
        //��ǥ�踦 n�� Ȯ���� �������� A* �˰����� �����Ѵ�.
        //�־��� ���� ��ǥ�踦 n�� Ȯ���� �� ���� ��ġ�� �����Ѵ�.
        public static (Vector3Int start, Vector3Int destination) 
            Encode(MapSPFA spfa, Vector3 start, Vector3 destination)
        {
            MapModel model = spfa.model;
            Matrix4x4 trMat = model.transform.ToMat();

            int n = spfa.resolution;
            start = trMat.MultiplyPoint(start) * n;
            destination = trMat.MultiplyPoint(destination) * n;

            Vector3Int convertedStart = start.ToVector3Int();
            Vector3Int convertedDestination = destination.ToVector3Int();

            return (convertedStart, convertedDestination);
        }
    }
}
