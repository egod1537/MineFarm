using Minefarm.Entity.Actor;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Entity.DropItem
{
    [RequireComponent(typeof(DropItemController))]
    public class DropItemModel : EntityModel
    {
        public ItemModel item;
        public EntityModel owner;

        private DropItemController _controller;
        public DropItemController controller { 
            get => _controller ??= GetComponent<DropItemController>();
        }
    }
}