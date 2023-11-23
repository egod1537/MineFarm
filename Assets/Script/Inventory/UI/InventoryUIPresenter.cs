using Minefarm.Item;
using Minefarm.Item.Consumption;
using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Inventory.UI
{
    public class InventoryUIPresenter : InventoryUIHahaviour
    {
        public UnityEvent onOpenInventory = new();
        public UnityEvent onCloseInventory = new();

        public UnityEvent<ItemCategory> onChangeCategory = new();
        public UnityEvent<int> onMoveSlot = new();
        public UnityEvent<ItemModel> onDropItem = new();
        public UnityEvent<ConsumptionItemModel> onUsedItem = new();

        public UnityEvent onUpdateStat = new();
        public UnityEvent onUpdateInventory = new();
        public UnityEvent<ItemModel> onUpdateInformation = new();
         
        private void Awake()
        {
            onChangeCategory.AddListener(category =>
            {
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });
            onMoveSlot.AddListener(pos => onUpdateInformation.Invoke(GetSlotItem()));

            inventory.equipment.onEquip.AddListener((equip) =>
            {
                onUpdateStat.Invoke();
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });
            inventory.equipment.onUnEquip.AddListener(equip =>
            {
                onUpdateStat.Invoke();
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });

            onDropItem.AddListener(item =>
            {
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });

            onUsedItem.AddListener(item =>
            {
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });

            player.inventory.onUpdate.AddListener(() =>
            {
                onUpdateStat.Invoke();
                onUpdateInventory.Invoke();
                onUpdateInformation.Invoke(GetSlotItem());
            });
        }

        private ItemModel GetSlotItem() => player.inventory[selectedCategory, selectedSlot];
    }
}

