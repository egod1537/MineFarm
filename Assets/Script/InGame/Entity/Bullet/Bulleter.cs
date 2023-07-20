using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Bullet
{
    public class Bulleter : Singletone<Bulleter>
    {
        public static BulletModel ShotBullet(
            ActorModel owner,
            BulletModelType bulletModelType, 
            Vector3 position,
            Vector3 direction,
            float range=1f,
            int damage=0,
            float speed=1.0f)
        {
            GameObject go = Instantiate(BulletDB.LoadBullet(bulletModelType));

            Transform tr = go.transform;
            tr.SetParent(ins.transform);
            tr.position = position;

            BulletModel bullet = go.GetComponent<BulletModel>();
            bullet.owner = owner;
            bullet.direction = direction;
            bullet.distance = range;
            bullet.damage = damage;

            bullet.body.transform.rotation = Quaternion.Euler(direction);

            return bullet;
        }
    }
}