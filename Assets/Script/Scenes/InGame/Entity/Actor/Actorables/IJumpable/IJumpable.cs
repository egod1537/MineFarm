
using UnityEngine;
namespace Minefarm.Entity.Actor.Interface
{
    public interface IJumpable
    {
        public const float JUMPPOWER_CONSTANT = 250f;
        public const float DELAY_JUMP = 0.5f;
        public const float DISTANCE_JUMP_CHECK = 0.5f;

        public bool Jump();
        public bool CanJump();
    }
}