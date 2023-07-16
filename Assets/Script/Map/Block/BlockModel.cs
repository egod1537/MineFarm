using Minefarm.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Map.Block
{
    [ExecuteInEditMode]
    public class BlockModel : EntityModel
    {
        private MapModel _mapModel;
        public MapModel mapModel {
            get
            {
                if (transform.parent == null) return null;
                return _mapModel ??= transform.parent.GetComponent<MapModel>();
            }
        }  

        public Vector3Int pos;
        public BlockID blockID;

        private void OnDestroy()
        {
            mapModel?.RemoveBlock(pos);
        }

        public void Destroy() => mapModel?.DestroyBlock(this.pos);
    }
}