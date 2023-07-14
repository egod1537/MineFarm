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

        private bool _isPixelized;
        public bool isPixelized { 
            get => _isPixelized;
            set => SetPixelize(value);
        }

        private void Awake()
        {
            isPixelized = true;
        }

        private void OnDestroy()
        {
            mapModel?.RemoveBlock(pos);
        }

        private void SetPixelize(bool flag)
        {
            _isPixelized = flag;

            int childCount = transform.childCount;
            for(int i=0; i < childCount; i++)
            {
                MeshRenderer meshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
                meshRenderer.material = 
                    flag ? BlockDB.LoadPixelizedMaterial(blockID) : BlockDB.LoadMaterial(blockID);
            }
        }

        public void Destroy() => mapModel?.DestroyBlock(this.pos);
    }
}