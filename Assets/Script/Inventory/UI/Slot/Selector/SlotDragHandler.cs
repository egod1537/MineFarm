using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Minefarm.InGame;

namespace Minefarm.Inventory.UI.Slot
{
    [HideInInspector]
    public class SlotDragHandler : InventoryUIHahaviour
    {
        const string PATH_SLOT_TEMPLATE = "UI/Inventory/Slot Template";
        const string TAG_SLOT = "Inventory@Slot";

        private SlotSelector selctor;
        private GraphicRaycaster raycaster;

        GameObject slotTemplate;
        GameObject dragObject;

        private void Awake()
        {
            selctor= GetComponent<SlotSelector>();
            raycaster = GetComponentInParent<GraphicRaycaster>();

            slotTemplate = Resources.Load<GameObject>(PATH_SLOT_TEMPLATE);

            SubscribleDragHandler();
        }

        private void SubscribleDragHandler()
        {
            bool isSuccessed = false;

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ =>
                {
                    PointerEventData point = new(null);
                    point.position = Input.mousePosition;

                    List<RaycastResult> rets = new();
                    raycaster.Raycast(point, rets);

                    if (rets.Count <= 0) return -1;
                    foreach (var ret in rets)
                        if (ret.gameObject.CompareTag(TAG_SLOT))
                            return int.Parse(ret.gameObject.name);
                    return -1;
                })
                .Where(slot => slot != -1)
                .Subscribe(slot =>
                {
                    selctor.SelectSlot(slot);
                    if(nowSlotItem != null)
                        OnSlotDragBegin(slot);
                    isSuccessed = true;
                });

            this.UpdateAsObservable()
                .Where(_ => isSuccessed)
                .Where(_ => Input.GetMouseButton(0))
                .Where(_ => nowSlotItem != null)
                .Subscribe(_ => OnSlotDrag());

            this.UpdateAsObservable()
                .Where(_ => isSuccessed)
                .Where(_ => Input.GetMouseButtonUp(0))
                .Where(_ => nowSlotItem != null)
                .Select(_ =>
                {
                    PointerEventData point = new(null);
                    point.position = Input.mousePosition;

                    List<RaycastResult> rets = new();
                    raycaster.Raycast(point, rets);

                    if (rets.Count <= 0) return -1;
                    foreach (var ret in rets)
                        if (ret.gameObject.CompareTag(TAG_SLOT))
                            return int.Parse(ret.gameObject.name);
                    return -1;
                })
                .Subscribe(slot =>
                {
                    OnSlotDragEnd(slot);
                    isSuccessed = false;
                });
        }

        public void OnSlotDragBegin(int slot)
        {
            dragObject = Instantiate(slotTemplate);

            Transform tr = dragObject.transform;
            tr.SetParent(transform.GetComponentInParent<Canvas>().transform);
            tr.localPosition = Vector3.zero;

            ViewSlotItem view = dragObject.GetComponent<ViewSlotItem>();
            view.item = nowSlotItem;
        }
        public void OnSlotDrag()
        {
            dragObject.transform.localPosition = 
                Input.mousePosition
                - new Vector3(Screen.width, Screen.height, 0f) * 0.5f;    
        }
        public void OnSlotDragEnd(int slot)
        {
            Destroy(dragObject);

            if (selectedSlot >= InventoryModel.INDEX_QUICK_SLOT ||
                slot >= InventoryModel.INDEX_QUICK_SLOT)
                return;

            if(slot != -1 && inventory.controller.Move(selectedCategory, selectedSlot, slot))
                selctor.SelectSlot(slot);
        }
    }
}

