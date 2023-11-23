using Minefarm.Entity.Actor.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    public class MapSPFAEncoder
    {   
        //n = spfa.resolution
        //좌표계를 n배 확장한 공간에서 A* 알고리즘을 진행한다.
        //주어진 월드 좌표계를 n배 확장한 맵 공간 위치로 변경한다.
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
