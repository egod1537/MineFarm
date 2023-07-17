using Minefarm.InGame;
using Minefarm.Map;
using Minefarm.Map.Block;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity
{
    public class PlayerController : ActorController
    {
        public override void Awake()
        {
            base.Awake();

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => base.Jump());

            this.UpdateAsObservable()
                .Select(_ =>
                {
                    float dx = Input.GetAxis("Horizontal"), dy = Input.GetAxis("Vertical");
                    Vector3 ret = new Vector3(dx + dy, 0f, dy - dx);
                    if (ret.sqrMagnitude > 1f) ret.Normalize();
                    return ret;
                })
                .Where(dir => !Mathf.Approximately(dir.sqrMagnitude, 0f))
                .Subscribe(dir => base.Move(dir));

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => Interactive());

            base.actorModel.onInteractive
                .Where(entity => entity is BlockModel)
                .Select(entity => (BlockModel) entity)
                .Subscribe(block => block.Destroy());
        }

        private void Update()
        {
            MapModel map = GameManager.ins.map;
            Vector3 to = map.WorldToMapIndex(transform.position);
            Vector3 a = map.transform.ToMat().MultiplyPoint(transform.position);
        }
    }
}

