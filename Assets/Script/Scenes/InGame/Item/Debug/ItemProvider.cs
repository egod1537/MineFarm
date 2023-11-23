using Minefarm.Entity.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Item
{
    public class ItemProvider : MonoBehaviour
    {
        public ActorModel target;
        public ItemID itemID;

        public void Provide()
        {
            target.inventory.controller.Add(Itemer.CreateItemModel(itemID, target));
        }
    }
}