using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Inventory.UI
{
    public class InventoryUIModel : Singletone<InventoryUIModel>
    {
        public ItemCategory selectedCategory;
        public int selectedSlot;

        private InventoryUIPresenter _presenter;
        public InventoryUIPresenter presenter {  
            get => _presenter ??= GetComponent<InventoryUIPresenter>();
        }  
    }
}