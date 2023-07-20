using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Dashable
{
    public interface IDashable
    {
        public const float DELAY_DASH = 0.5f;
        public const float CONSTANT_DASH_FORCE = 10f;
        public bool Dash(Vector3 direction);
        public bool CanDash(Vector3 direction);
    }
}