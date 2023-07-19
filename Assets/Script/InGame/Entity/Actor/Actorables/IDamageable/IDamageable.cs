using UnityEngine;

namespace Minefarm.Entity.Actor.Interface
{
    public interface IDamageable
    {
        public bool Damage(ActorModel target, int damage, out int retDamage, out bool isCritical);
    }
}
