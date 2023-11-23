using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UniRx;
using Minefarm.Inventory.UI;

namespace Minefarm.InGame.UI.Bottom
{
    public class QuickSlotSelector : MineBehaviour
    {
        public int selectedQuickSlot;
        public Transform cursor;
        List<Transform> slots;

        private void Awake()
        {
            InitializeSlots();

            this.UpdateAsObservable()
                .Select(_ => InputHandler.GetQuickSlot())
                .Where(num => num != -1)
                .Subscribe(num => SetSlot(num));

            InventoryUIModel.ins.presenter
                .onCloseInventory.AddListener(() => SetSlot(selectedQuickSlot));
            InventoryUIModel.ins.presenter
                .onMoveSlot.AddListener(slot => SetSlot(selectedQuickSlot));
        }

        private void Start()
        {
            SetSlot(0);
        }

        private void InitializeSlots()
        {
            int sz = transform.childCount;
            slots = new();
            for (int i = 0; i < sz; i++)
                slots.Add(transform.GetChild(i));
        }

        public void SetSlot(int slot)
        {
            cursor.DOMove(slots[slot].position, 0.1f);

            selectedQuickSlot = slot;
            player.SetHandleItem(player.inventory.quick[slot]);

            InventoryUIModel.ins.presenter.onUpdateStat.Invoke();
        }
    }
}