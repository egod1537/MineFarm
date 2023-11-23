using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Minefarm.Inventory.UI;

namespace Minefarm.InGame.UI
{
    public class OpenInventory : InventoryUIHahaviour
    {
        public bool isOpen;

        CanvasGroup canvasGroup;
        float delayOpen;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();  

            this.UpdateAsObservable()
                .Where(_ => delayOpen <= 0f)
                .Where(_ => Input.GetKeyDown(KeyCode.E))
                .Where(_ => !isOpen)
                .Subscribe(_ =>
                {
                    canvasGroup.DOFade(1f, 0.5f);
                    isOpen = true;
                    delayOpen = 0.25f;

                    presenter.onOpenInventory.Invoke();
                });

            this.UpdateAsObservable()
                .Where(_ => delayOpen <= 0f)
                .Where(_ => Input.GetKeyDown(KeyCode.E))
                .Where(_ => isOpen)
                .Subscribe(_ =>
                {
                    canvasGroup.DOFade(0f, 0.5f);
                    isOpen = false;
                    delayOpen = 0.25f;

                    presenter.onCloseInventory.Invoke();
                });

            this.UpdateAsObservable()
                .Subscribe(_ => delayOpen -= Time.deltaTime);
        }
    }
}