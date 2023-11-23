using Minefarm.Entity.Actor;
using Minefarm.Entity.Bullet;
using Minefarm.Entity.DropItem;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity
{
    public class Spawner : Singletone<Spawner>
    {
        private static Transform actors;
        private static Transform bullets;
        private static Transform dropItems;

        private void Awake()
        {
            actors = CreateSpawnGroup("Actors");
            bullets = CreateSpawnGroup("Bullets");
            dropItems = CreateSpawnGroup("Drop Items");
        }

        private Transform CreateSpawnGroup(string name)
        {
            Transform ret = GameObject.Find(name)?.transform;
            if(ret == null)
            {
                ret = new GameObject(name).transform;
                ret.name = name;
                ret.SetParent(this.transform);
            }
            return ret;
        }

        const float MIN_POWER_DROP_ITEM = 100f;
        const float MAX_POWER_DROP_ITEM = 300f;

        public static DropItemModel DropItem(Vector3 pos, ItemID itemID, int count = 1)
        {
            GameObject go = Instantiate(ItemDB.GetDropItemModel());

            Transform tr = go.transform;
            tr.SetParent(dropItems);
            tr.position = pos;

            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            Vector3 dir = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
            dir.Normalize();
            dir *= Random.Range(0.3f, 0.5f);

            float power = Random.Range(MIN_POWER_DROP_ITEM, MAX_POWER_DROP_ITEM);
            rigidbody.AddForce((dir + Vector3.up) * power);

            DropItemModel model = go.GetComponent<DropItemModel>();
            model.item = Itemer.CreateItemModel(itemID, null);
            model.item.count = count;

            return model;
        }
        public static List<DropItemModel> DropItems(Vector3 pos, List<ItemID> itemIDs)
        {
            List<DropItemModel> ret = new List<DropItemModel>();
            foreach (var item in itemIDs)
                ret.Add(DropItem(pos, item));
            return ret;
        }
        public static DropItemModel DropItem(Vector3 pos, ItemModel itemModel)
            => DropItem(pos, itemModel.itemID, itemModel.count);

        public static BulletModel ShotBullet(
            ActorModel owner,
            BulletModelType bulletModelType,
            Vector3 position,
            Vector3 direction,
            float range = 1f,
            int damage = 0,
            float speed = 1.0f)
        {
            GameObject go = Instantiate(BulletDB.LoadBullet(bulletModelType));

            Transform tr = go.transform;
            tr.SetParent(bullets);
            tr.position = position;

            BulletModel bullet = go.GetComponent<BulletModel>();
            bullet.owner = owner;
            bullet.direction = direction;
            bullet.distance = range;
            bullet.damage = damage;

            bullet.body.transform.rotation = Quaternion.LookRotation(direction);

            return bullet;
        }

        public static ActorModel CreateActor(
            EntityID actorID,
            Vector3 position)
        {
            GameObject go = Instantiate(ActorDB.GetActorModel(actorID));

            Transform tr = go.transform;
            tr.SetParent(actors);
            tr.position = position;

            ActorModel actorModel = go.GetComponent<ActorModel>();
            return actorModel;
        }
    }
}