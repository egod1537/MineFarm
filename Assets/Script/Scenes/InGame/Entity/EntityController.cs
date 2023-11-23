using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Minefarm.Map;
using Minefarm.InGame;

namespace Minefarm.Entity
{
    [RequireComponent(typeof(EntityModel))]
    public class EntityController : MonoBehaviour
    {
        private EntityModel _entityModel;
        protected EntityModel entityModel { get => _entityModel ??= GetComponent<EntityModel>(); }

        private Rigidbody _rigidbody;
        protected Rigidbody rigidbody { get=>_rigidbody ??= GetComponent<Rigidbody>(); }

        public MapModel map { get => GameManager.ins.map; }

        public void Start()
        {
            Spawn();
        }

        public virtual void Spawn()
        {
            entityModel.isLive = true;
            entityModel.onSpawn.Invoke();
        }

        public virtual void Kill()
        {
            entityModel.isLive = false;
            entityModel.onDead.Invoke();
        }
    }
}