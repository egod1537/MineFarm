using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Utilty
{
    public class FollwingCamera : MonoBehaviour
    {
        public Transform target;
        [Range(0f, 1f)]
        public float ratio;

        public Vector3 targetRotation;
        public float targetDistance;
        public float follwingDistance;

        public bool isImmediatelyFollowing;

        bool flagMove;

        private void LateUpdate()
        {
            if (isImmediatelyFollowing)
                transform.position = GetFollowingPosition();
            else
            {
                float distance = (transform.position - target.position).sqrMagnitude;
                float dl = targetDistance - follwingDistance, dr = targetDistance + follwingDistance;
                dl *= dl; dr *= dr;

                if (distance < dl || dr < distance) flagMove = true;

                if (flagMove)
                    transform.position = Vector3.Lerp(
                        transform.position,
                        GetFollowingPosition(),
                        ratio * Time.deltaTime * 3f);

                float d = targetDistance * targetDistance;
                if (Mathf.Approximately(d, distance))
                    flagMove = false;
            }
        }

        public Vector3 GetFollowingPosition()
            => target.position - GetPositionOnSphere(targetRotation, targetDistance);

        public Vector3 GetPositionOnSphere(Vector3 rotation, float radius)
        {
            float xt = rotation.x * Mathf.Deg2Rad, yt = rotation.y * Mathf.Deg2Rad;
            return radius * new Vector3(
                Mathf.Cos(xt) *Mathf.Cos(yt),
                Mathf.Sin(xt),
                Mathf.Cos(xt) *Mathf.Sin(yt));
        }
    }
}
