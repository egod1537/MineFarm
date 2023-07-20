using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Entity.Item
{
    public class Droper : Singletone<Droper>
    {
        const float MIN_POWER_DROP_ITEM = 100f;
        const float MAX_POWER_DROP_ITEM = 300f;

        public static DropItemModel DropItem(ItemID itemID, ActorModel owner, Vector3 pos)
        {
            GameObject go = Instantiate(ItemDB.GetItemModel(itemID));

            Transform tr = go.transform;
            tr.SetParent(ins.transform);
            tr.position = pos;

            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            Vector3 dir = new Vector3(Random.Range(0f, 1f) ,0f, Random.Range(0f, 1f));
            dir.Normalize();
            dir *= Random.Range(0.3f, 0.5f);
            float power = Random.Range(MIN_POWER_DROP_ITEM, MAX_POWER_DROP_ITEM);
            rigidbody.AddForce((dir + Vector3.up)*power);

            DropItemModel model = go.GetComponent<DropItemModel>();
            model.owner = owner;

            return model;
        }
        public static List<DropItemModel> DropItems(List<ItemID> itemIDs, ActorModel owner, Vector3 pos)
        {
            List<DropItemModel> ret = new List<DropItemModel>();
            foreach (var item in itemIDs)
                ret.Add(DropItem(item, owner, pos));
            return ret;
        }
    }
}

