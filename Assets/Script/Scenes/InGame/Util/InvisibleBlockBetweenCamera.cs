using Minefarm.Entity.Actor.Block;
using Minefarm.Map.Block;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.InGame.Util
{
    public class InvisibleBlockBetweenCamera : MonoBehaviour
    {
        public Transform target;
        public float radius;
        public float near;

        private void Awake()
        {
            this.UpdateAsObservable()
                .Select(_ =>
                {
                    float distance = Vector3.Distance(transform.position, target.position)-near;
                    Vector3 dir = (target.position - transform.position).normalized;
                    RaycastHit[] hits = Physics.SphereCastAll(
                        transform.position,
                        radius,
                        dir,
                        distance,
                        1<<LayerMask.NameToLayer("Block"));
                    List<BlockView> ret = new();
                    foreach (var hit in hits) ret.Add(hit.transform.GetComponent<BlockView>());
                    return ret;
                }).
                Subscribe(views =>
                {
                    foreach (var view in views) view.SetInvisible(true);
                });

            this.UpdateAsObservable()
                .Select(_ =>
                {
                    Vector3 dir = (target.position - transform.position).normalized;
                    RaycastHit[] hits = Physics.SphereCastAll(
                        transform.position,
                        radius,
                        dir,
                        float.MaxValue,
                        1 << LayerMask.NameToLayer("Block"));
                    List<BlockView> ret = new();
                    foreach (var hit in hits)
                    {
                        BlockView view = hit.transform.GetComponent<BlockView>();
                        if (view == null) continue;
                        ret.Add(view);
                    }
                    return ret;
                }).
                Subscribe(views =>
                {
                    foreach (var view in views)
                        view.SetReveal();
                });
        }
    }
}