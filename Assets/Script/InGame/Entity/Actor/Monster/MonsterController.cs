using Minefarm.InGame;
using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
namespace Minefarm.Entity.Monster
{
    public class MonsterController : ActorController
    {
        const float DISTANCE_INTERPOLATE = 0.05f;
        
        private MapModel map { get => GameManager.ins.map; }

        public EntityModel target;
        [SerializeField]
        public List<Vector3> wayPoint = new();
        public int nowWayPoint;

        List<Vector3Int> ways = new();

        public override void Awake()
        {
            base.Awake();
            InvokeRepeating("UpdateRoute", 0f, 1f);
        }

        public void Update()
        {
            ProcessRouting();
        }

        public void OnDrawGizmos()
        {
            MapAlgorithm.DrawPath(map, ways);
        }

        public void ProcessRouting()
        {
            if (target == null) return;
            if (nowWayPoint >= wayPoint.Count)
            {
                wayPoint.Clear();
                nowWayPoint = 0;
                return;
            }

            Vector3 to = wayPoint[nowWayPoint];
            Vector3 diff = to - transform.position;

            if (diff.sqrMagnitude <= DISTANCE_INTERPOLATE * DISTANCE_INTERPOLATE)
                nowWayPoint++;
            else
            {
                //diff.y = 0f;
                Move(diff.normalized);
            }
        }

        public void UpdateRoute()
        {
            Vector3Int start = actorModel.mapPos + Vector3Int.up, 
                end = target.mapPos + Vector3Int.up;

            ways =  MapAlgorithm.ShortestPath(map,start, end);

            if (ways == null) return;

            wayPoint = new();
            foreach (var point in ways)
                wayPoint.Add(map.MapIndexToWorldPosition(point));

            nowWayPoint = 0;
        }
    }
}