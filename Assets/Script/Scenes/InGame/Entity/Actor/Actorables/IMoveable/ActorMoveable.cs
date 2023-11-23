using Minefarm.Entity.Actor.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Minefarm.Entity.Actor.Moveable
{
    public class ActorMoveable : Actorable, IMoveable
    {
        public ActorMoveable(ActorModel actor) : base(actor)
        {
        }

        public virtual bool Move(Vector3 direction)
        {
            LookAt(direction, IMoveable.SPEED_ROTATION * Time.deltaTime);
            if (!CanMove(direction)) return false;

            float speedPower = Time.deltaTime * actor.stats.calculatedSpeed * IMoveable.SPEED_CONSTANT;

            rigidbody.MovePosition(rigidbody.position +direction * speedPower);
            return true;
        }

        public virtual bool CanMove(Vector3 direction)
        {
            float speedPower = Time.deltaTime * actor.stats.calculatedSpeed * IMoveable.SPEED_CONSTANT;
            return controller.GetFowardEntity(
                direction, 
                speedPower, 
                LayerMask.NameToLayer("Block")) == null;
        }

        protected void LookAt(Vector3 dir, float maxRadianDelta)
        {
            Vector3 rotationDir = Vector3.RotateTowards(
                actor.body.forward,
                dir,
                maxRadianDelta,
                0f);
            rotationDir.y = 0f;
            actor.body.rotation = Quaternion.LookRotation(rotationDir);
        }
    }
}
