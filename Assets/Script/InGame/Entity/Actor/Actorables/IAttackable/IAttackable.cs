
using UnityEngine;
namespace Minefarm.Entity.Actor.Interface
{
    public interface IAttackable
    {
        public bool Attack(ActorModel target, int damage);
        public bool CanAttack(ActorModel target);
    }
}
