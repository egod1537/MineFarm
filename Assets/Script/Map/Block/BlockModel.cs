using Minefarm.Entity;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Damageable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Block
{
    [ExecuteInEditMode]
    public class BlockModel : ActorModel
    {
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