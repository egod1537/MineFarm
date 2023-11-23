using UnityEngine;
using UniRx;
using UnityEngine.Events;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Entity.Actor.Jumpable;
using Minefarm.Entity.Actor.Moveable;
using Minefarm.Entity.Actor.Attackable;
using Minefarm.Map.Block;
using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.Entity.Actor.Damageable;
using Minefarm.Entity.Bullet;
using Minefarm.Inventory;
using Minefarm.Item.Equipment;
using Minefarm.Entity.Actor.Block;
using Minefarm.Item;

namespace Minefarm.Entity.Actor
{
    [RequireComponent(typeof(ActorController))]
    public class ActorModel : EntityModel
    {
        public UnityEvent<Vector3> onMove = new();
        public UnityEvent onJump = new();

        public UnityEvent<ItemModel> onSetHandleItem = new();

        public UnityEvent<EntityModel> onFowardAction = new();
        public UnityEvent<Vector3> onShoot = new();
        public UnityEvent<BlockModel, int> onDig = new();
        public UnityEvent<BlockModel> onBreak = new();

        public UnityEvent<ActorModel, int> onAttack = new();
        public UnityEvent<ActorModel> onKillEntity = new();
        public UnityEvent<ActorModel, int, bool> onDamage = new();

        public UnityEvent<int> onLevel = new();
        public UnityEvent<int> onLevelUp = new();
        public UnityEvent<long> onExp = new();

        public UnityEvent<int> onHp = new();

        [SerializeField]
        public ActorInfo info;

        [SerializeField]
        public Stats stats;

        public int level
        {
            get => info.level;
            set
            {
                if (info.level < value) onLevelUp.Invoke(value);
                if (info.level != value) onLevel.Invoke(info.level);
                info.level = value;
            }
        }

        public long exp
        {
            get => info.exp;
            set
            {
                bool chk = info.exp != value;
                info.exp = value;
                if (chk) onExp.Invoke(info.exp);
            }
        }

        public int gold
        {
            get => info.gold;
            set => info.gold = value;
        }

        public int hp {
            get => info.hp; 
            set
            {
                if (info.hp != value) onHp.Invoke(value);
                info.hp = value;
            } 
        }

        [SerializeField]
        public InventoryModel inventory;
        public EquipmentItemFrame equipment { get => inventory.equipment; }

        public IFowardActionable fowardActionable;

        public IAttackable attackable;

        public IMoveable moveable;
        public IDamageable damageable;
        public IJumpable jumpable;

        public bool isGround;
        public bool isDead { get => isLive && hp <= 0; }

        public ItemModel handleItem { get; private set; }

        public ActorController actorController { 
            get => (ActorController) base.entityController;
        }
        public Vector3 centerPosition { get => transform.position + Vector3.up * 0.5f; }

        public void Awake()
        {
            inventory = new InventoryModel(this);

            jumpable = new ActorJumpable(this);
            moveable = new ActorMoveable(this);

            attackable = new ActorAttackable(this);
            damageable = new ActorDamageable(this);
        }
        public virtual long GetNextLevelExp() { return long.MaxValue; }

        public virtual void SetHandleItem(ItemModel item)
        {
            handleItem = item;
            onSetHandleItem.Invoke(item);
        }
    }
}