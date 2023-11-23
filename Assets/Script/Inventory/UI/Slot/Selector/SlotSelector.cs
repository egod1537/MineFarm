using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minefarm.Inventory.UI.Slot
{
    [RequireComponent(typeof(SlotDragHandler))]
    public class SlotSelector : InventoryUIHahaviour
    {
        public Transform cursor;
        List<Transform> slotTransform;

        private void Awake()
        {
            slotTransform = new List<Transform>();
            int sz = transform.childCount;
            for (int i = 0; i < sz; i++) slotTransform.Add(transform.GetChild(i));
        }

        private void Start()
        {
            RenameSlots();
            SelectSlot(0);
        }

        public void RenameSlots()
        {
            for (int i = 0; i < slotTransform.Count; i++)
                slotTransform[i].gameObject.name = $"{i}";
        }
        
        public void SelectSlot(int slot)
        {
            cursor.DOMove(slotTransform[slot].position, 0.1f);

            selectedSlot = slot;
            presenter.onMoveSlot.Invoke(slot);
        }
    }
}