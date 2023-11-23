using Minefarm.Map;
using Minefarm.Map.Algorithm.SPFA;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster.Controller
{
    public class MonsterMoveController 
    {
        const float DISTANCE_RECALCULATE = 0.5f;
        const float SQR_DISTANCE_RECALCULATE = DISTANCE_RECALCULATE* DISTANCE_RECALCULATE;

        const float DELAY_REROUTE = 0.25f;

        private MonsterController controller;
        private MonsterModel actor { get => controller.actorModel as MonsterModel; }
        private ActorModel target { get => controller.target; }
        private MapModel map { get => controller.map; }

        private Vector3 lastTargetPosition = -Vector3.one;
        private List<Vector3> wayPoint = new();
        private int nowWayPoint;

        float delayReroute = 0.0f;
        Vector3 fowardDirection;

        public MonsterMoveController(MonsterController controller)
        {
            this.controller = controller;
            fowardDirection = actor.transform.forward;
        }

        public void Update()
        {
            delayReroute -= Time.deltaTime;
            if (target == null || actor.hp <= 0) return;
            if ((lastTargetPosition - target.transform.position).sqrMagnitude >
                SQR_DISTANCE_RECALCULATE && delayReroute <= 0.0f)
                CalculateShortestPath();

            if(nowWayPoint < wayPoint.Count)
            {
                MoveToWayPoint(nowWayPoint);
                if (IsDestinateWayPoint(nowWayPoint))
                    nowWayPoint++;
            }
        }

        private void CalculateShortestPath()
        {
            wayPoint = map.pathFinder.Query(
                controller.transform.position, 
                target.transform.position);
            nowWayPoint = 0;
            lastTargetPosition = target.transform.position;
            delayReroute = DELAY_REROUTE;
        }

        private bool IsDestinateWayPoint(int numWayPoint)
            => (actor.transform.position - wayPoint[numWayPoint]).sqrMagnitude <
            controller.distanceEpsilon * controller.distanceEpsilon;

        private void MoveToWayPoint(int numWayPoint)
        {
            Vector3 dir = (wayPoint[numWayPoint] - actor.transform.position).normalized;
            fowardDirection = Vector3.RotateTowards(fowardDirection, dir, 
                controller.maxMoveRotatingAngle * Time.deltaTime, 1f);
            controller.Move(fowardDirection);
        }
    }
}