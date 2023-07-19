using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Bullet
{
    public class BulletModel : EntityModel

    {
        public ActorModel owner;
        public int damage;

        public Vector3 direction;
        public float speed;
    }
}