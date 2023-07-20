using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Map
{
    public class MapAlgorithmExecuter : MonoBehaviour
    {
        public MapModel mapModel;
        public Vector3Int start, end;
        public List<Vector3Int> poses = new List<Vector3Int>();

        public void ShortestPath()
        {
            if(mapModel != null)
                poses = MapAlgorithm.ShortestPath(mapModel, start, end);
        }
        public void ClearShortestPath()
        {
            poses?.Clear();
        }

        public void OnDrawGizmosSelected()
        {
            if (mapModel != null)
                MapAlgorithm.DrawPath(mapModel, poses);
        }
    }
}

