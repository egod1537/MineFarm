using GoogleSheet.Type;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Item.Actionable;
using Minefarm.Item.Digable;
using Minefarm.Item.Shootable;
using System;
using UnityEngine;

namespace Minefarm.Item
{
    [Serializable]
    public class ItemModel
    {
        public ItemCategory category;
        public ItemID itemID;

        public ActorModel owner { get; private set; }

        [SerializeField]
        public Stats stat = new Stats();

        public int maxCount = 64;
        public int count = 1;
        public int remainCount { get => maxCount - count; }

        public ItemController controller;

        public IShootable shootable;
        public IDigable digable;

        public ItemModel(ActorModel owner)
        {
            shootable = new DefaultShootable(owner);
            digable = new DefaultDigable(owner);

            controller = new ItemController(this);
            controller.actionable.Add(new DropItemAction());

            SetOwner(owner);
        }

        public T Clone<T>() where T : ItemModel, new()
        {
            T ret = new T();
            ret.category = category;
            ret.itemID = itemID;

            ret.stat = stat.Clone();

            ret.maxCount = maxCount;
            ret.count = count;

            ret.controller = controller;
            return ret;
        }

        public void SetOwner(ActorModel owner)
        {
            this.owner = owner;
            shootable.SetActor(owner);
            digable.SetActor(owner);
        }
    }
}

