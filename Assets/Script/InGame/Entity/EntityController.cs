using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace Minefarm.Entity
{
    [RequireComponent(typeof(EntityModel))]
    public class EntityController : MonoBehaviour
    {
        private EntityModel _entityModel;
        protected EntityModel entityModel { get => _entityModel ??= GetComponent<EntityModel>(); }

        public virtual void Awake()
        {
            this.UpdateAsObservable()
                .Where(_ => entityModel.isLive && entityModel.hp <= 0)
                .Subscribe(_ => Death());
        }

        public virtual void Spawn()
        {
            entityModel.isLive = true;
            entityModel.hp = entityModel.GetMaxHp();
            entityModel.onSpawn.Invoke();
        }

        public virtual void Death()
        {
            entityModel.isLive = false;
            entityModel.onDeath.Invoke();
        }
    }
}