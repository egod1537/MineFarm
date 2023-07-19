using UniRx;
using UnityEngine;
namespace Minefarm.Entity.Actor
{
    public class Actorable
    {
        protected ActorModel actor;
        protected ActorController controller { get => actor.actorController; }
        protected GameObject gameObject { get => actor.gameObject; }
        protected Transform transform { get => actor.transform; }

        private Rigidbody _rigidbody;
        protected Rigidbody rigidbody { get => _rigidbody ??= gameObject.GetComponent<Rigidbody>(); }
        public Actorable(ActorModel actor)
        {
            this.actor = actor;
        }
    }
}