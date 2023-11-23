using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Algorithm.SPFA
{
    [Serializable]
    public class MapSPFA
    {
        public int resolution = 1;
        public Vector3 start, destination;
        [SerializeField]
        public MapModel model;

        [SerializeField]
        public List<Vector3> cache = new();

        [SerializeField]
        public MapSPFAViewer viewer;

        public MapSPFA()
        {
            viewer = new MapSPFAViewer(this);
        }
        public MapSPFA(MapModel model)
        {
            this.model = model;
            viewer = new MapSPFAViewer(this);
        }

        public List<Vector3> Query() => Query(this.start, this.destination);
        public List<Vector3> Query(Vector3 start, Vector3 destination)
        {
            var input = MapSPFAEncoder.Encode(this, start, destination);
            var route = MapSPFAAlgorithm.Run(this, input.start, input.destination);
            SetCache(route);
            var path = MapSPFADecoder.Decode(this, route);
            return path;
        }

        public void OnDrawGizmosSelected()
        {
            viewer.OnDrawGizmosSelected();
        }

        private void SetCache(List<Vector3Int> route)
        {
            int n = resolution;
            cache = new();
            foreach(var pos in route)
                cache.Add(new Vector3(pos.x, pos.y, pos.z) / n);
        }
    }
}