
using UnityEngine;
namespace Minefarm.Entity.Actor.Interface
{
    public interface IMoveable
    {
        const float SPEED_CONSTANT = 5f;
        const float SPEED_ROTATION = 30f;
        public bool Move(Vector3 direction);
        public bool CanMove(Vector3 direction);
    }
}
