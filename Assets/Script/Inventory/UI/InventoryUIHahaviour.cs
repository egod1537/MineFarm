using JetBrains.Annotations;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Inventory.UI
{
    public class InventoryUIHahaviour : MineBehaviour
    {
        protected ItemCategory selectedCategory
        {
            get => InventoryUIModel.ins.selectedCategory;
            set => InventoryUIModel.ins.selectedCategory = value;
        }

        protected int selectedSlot
        {
            get => InventoryUIModel.ins.selectedSlot;
            set => InventoryUIModel.ins.selectedSlot = value;
        }

        protected InventoryModel inventory
        {
            get => player.inventory;
            set => player.inventory = value;
        }

        protected ItemModel nowSlotItem
        {
            get => inventory[selectedCategory, selectedSlot];
            set => inventory[selectedCategory, selectedSlot] = value;
        }

        protected InventoryUIPresenter presenter { get => InventoryUIModel.ins.presenter; }
    }
}