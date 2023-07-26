using Minefarm.Entity;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Damageable;
using Minefarm.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Entity.Actor.Block
{
    [ExecuteInEditMode]
    public class BlockModel : ActorModel
    {
        public UnityEvent onInvisible = new();
        public UnityEvent onRecovery = new();

        private MapModel _mapModel;
        public MapModel mapModel { get => _mapModel ??= GetComponentInParent<MapModel>(); }

        public Vector3Int pos;
        public BlockID blockID;

        public void Awake()
        {
            base.Awake();
            damageable = new BlockDamageable(this);
        }

        private void OnDestroy()
        {
            mapModel?.RemoveBlock(pos);
        }

        public void Destroy() => mapModel?.DestroyBlock(this.pos);
    }
}