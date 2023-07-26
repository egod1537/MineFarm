using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster
{
    public class MonsterController : ActorController
    {
        const float DISTANCE_INTERPOLATE = 0.05f;

        public int stopDistance = 2;

        public ActorModel target;
        public Vector3Int targetPos { get => target.mapPos+Vector3Int.up; }
        public List<Vector3Int> wayPoint = new();
        public List<Vector3> wayWorldPoint = new();

        public int nowWayPoint;

        Vector3Int prvTargetPos = -Vector3Int.one;

        public override void Awake()
        {
            base.Awake();

            this.UpdateAsObservable()
                .Where(_ => target != null && prvTargetPos != targetPos)
                .Subscribe(_ => UpdateWayPoint());

            this.UpdateAsObservable()
                .Where(_ => IsRoute())
                .Where(_ => GetDistanceFromTarget() > stopDistance)
                .Subscribe(_ => ProcessRouting());

            actorModel.onKill.AddListener(() => Destroy(gameObject, 1f));
        }
        public void OnDrawGizmos()
        {
            MapAlgorithm.DrawPath(map, wayPoint);
        }
        public void ProcessRouting()
        {
            if (nowWayPoint >= wayPoint.Count)
            {
                wayPoint.Clear();
                nowWayPoint = 0;
                return;
            }

            Vector3 to = wayWorldPoint[nowWayPoint];
            Vector3 diff = to - transform.position;

            if (diff.sqrMagnitude <= DISTANCE_INTERPOLATE * DISTANCE_INTERPOLATE)
                nowWayPoint++;
            else
                Move(diff.normalized);
        }
        public int GetDistanceFromTarget() => wayPoint.Count - nowWayPoint;
        public void UpdateWayPoint()
        {
            Vector3Int start = actorModel.mapPos + Vector3Int.up, end = targetPos;

            wayPoint = map.GetShortestPath(start, end);
            if (wayPoint.Count == 0) return;

            wayWorldPoint = new();
            foreach (var point in wayPoint)
                wayWorldPoint.Add(map.MapIndexToWorldPosition(point) - Vector3.up * 0.5f);

            nowWayPoint = 0;
            prvTargetPos = end;
        }
        public bool IsRoute() => target != null && wayPoint.Count > 0;
    }
}