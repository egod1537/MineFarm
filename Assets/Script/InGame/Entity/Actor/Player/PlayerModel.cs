using Minefarm.Entity.Actor.Breakable;
using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.Entity.Actor.Interface;
using Minefarm.InGame;
using Minefarm.Item.Equipment;
using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Entity.Actor.Player
{
    public class PlayerModel : ActorModel
    {
        public UnityEvent<BlockModel, int> onDig = new();
        public UnityEvent<BlockModel> onBreak = new();

        public EquipmentFrame equipment;
        public IDigable digable;

        public void Awake()
        {
            base.Awake();

            digable = new PlayerDigable(this);
            fowardActionable = new PlayerFowardActionable(this);

            equipment = new EquipmentFrame(this);
        }
    }
}