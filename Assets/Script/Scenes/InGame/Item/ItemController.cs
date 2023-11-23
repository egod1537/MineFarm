using Minefarm.Item.Actionable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item
{
    public class ItemController
    {
        private ItemModel model;
        public List<IItemActionable> actionable;
        
        public ItemController(ItemModel model)
        {
            this.model = model;
            actionable= new List<IItemActionable>();
        }
    }
}