using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Actor.Dashable;
using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Entity.Bullet;
using Minefarm.InGame.Level;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Entity.Actor.Player
{
    public class PlayerModel : ActorModel
    {
        public const int MAX_DASH_COUNT = 5;

        public UnityEvent onDash = new();
        public UnityEvent onDashEnd = new();
        public UnityEvent onDashCharged = new();

        [SerializeField]
        private int _dashCount;
        public int dashCount
        {
            get => _dashCount;
            set => _dashCount = Mathf.Clamp(value, 0, MAX_DASH_COUNT);
        }
        public float dashRecycleDelay;

        public IDashable dashable;

        public void Awake()
        {
            base.Awake();

            dashable = new PlayerDashable(this);
            fowardActionable = new PlayerFowardActionable(this);
        }

        public override long GetNextLevelExp()
            => LevelDB.GetNextExp(level);

        public override void SetHandleItem(ItemModel item)
        {
            StatInjector.DejectItem(this, handleItem);
            StatInjector.InjectItem(this, item);

            stats.bulletModel = 
                item != null ? item.stat.bulletModel : BulletModelType.Melee;

            base.SetHandleItem(item);
        }
    }
}