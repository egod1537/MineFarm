using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Shootable
{
    public interface IShootable
    {
        public bool Shoot(Vector3 foward);
        public bool CanShoot();
    }
}
