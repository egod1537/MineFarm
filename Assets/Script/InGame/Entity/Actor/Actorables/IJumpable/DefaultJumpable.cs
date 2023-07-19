using Minefarm.Entity.Actor.Interface;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Networking;

namespace Minefarm.Entity.Actor.Jumpable
{
    public class DefaultJumpable : Actorable, IJumpable
    {
        private float jumpDelay;

        public DefaultJumpable(ActorModel actor) : base(actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ => jumpDelay -= Time.deltaTime);
        }

        public virtual bool CanJump() => jumpDelay <= 0f && actor.isGround;

        public virtual bool Jump()
        {
            if (!CanJump()) return false;
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0f;
            rigidbody.velocity = velocity;

            rigidbody.AddForce(
                transform.up * actor.jumpPower * IJumpable.JUMPPOWER_CONSTANT);

            jumpDelay = IJumpable.DELAY_JUMP;
            actor.isGround = false;

            return true;
        }
    }
}

