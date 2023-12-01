using Minefarm.Entity;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Damageable;
using Minefarm.Map;
using Minefarm.Map.Algorithm.Culling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
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

        public ReactiveProperty<bool> visible = new();
        public ReactiveProperty<bool> reveal = new();

        public List<Vector3> vertices;

        public void Awake()
        {
            base.Awake();
            visible.Value = true;

            damageable = new BlockDamageable(this);

            StatInjector.InjectBlock(this);
        }

        public void Initialize()
        {
            InitializeVertices();
        }

        private void InitializeVertices()
        {
            if (!Application.isPlaying) return;
            vertices = body.GetComponent<MeshFilter>().mesh.vertices.ToList();
            Camera mainCamera = Camera.main;

            for(int i=0; i < vertices.Count;i++)
                vertices[i] = transform.localToWorldMatrix.MultiplyPoint(vertices[i]);
        }

        private void OnDestroy()
        {
            mapModel?.RemoveBlock(pos);
        }

        public void Destroy() => mapModel?.DestroyBlock(this.pos);
    }
}