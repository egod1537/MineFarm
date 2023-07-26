using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.UI.InGame.Minimap
{
    public class ViewExtendMinimap : MonoBehaviour
    {
        public float extendRatio;
        public bool isExtend;
        private void Awake()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Tab))
                .Subscribe(_ =>
                {
                    isExtend = !isExtend;
                    transform.localScale = Vector3.one * (isExtend ? extendRatio : 1f);
                });
        }
    }
}