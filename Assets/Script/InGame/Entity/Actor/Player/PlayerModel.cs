using Minefarm.Entity.Actor.Block;
using Minefarm.Entity.Actor.Breakable;
using Minefarm.Entity.Actor.Dashable;
using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Entity.Item.Equipment;
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

        public UnityEvent<BlockModel, int> onDig = new();
        public UnityEvent<BlockModel> onBreak = new();

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

        public EquipmentFrame equipment;
        public IDigable digable;
        public IDashable dashable;

        public void Awake()
        {
            base.Awake();

            digable = new PlayerDigable(this);
            dashable = new PlayerDashable(this);
            fowardActionable = new PlayerFowardActionable(this);

            equipment = new EquipmentFrame(this);
        }
    }
}