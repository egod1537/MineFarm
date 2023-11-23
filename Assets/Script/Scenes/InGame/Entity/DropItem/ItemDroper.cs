using Minefarm.Entity.Actor;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Minefarm.Entity.DropItem
{
    public class ItemDroper : MonoBehaviour
    {
        public ItemID itemID;
        public int count;

        ActorModel actor;
        WaitForSeconds wfs = new(0.1f);

        private void Awake()
        {
            actor = gameObject.AddComponent<ActorModel>();
        }

        public void Drop()
        {
            IEnumerator enumerator()
            {
                for(int i=0; i < count; i++)
                {
                    Spawner.DropItem(transform.position, itemID);
                    yield return wfs;
                }
            }
            StartCoroutine(enumerator());
        }
    }
}