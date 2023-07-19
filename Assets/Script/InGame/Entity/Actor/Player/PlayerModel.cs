using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.InGame;
using Minefarm.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minefarm.Entity.Actor.Player
{
    public class PlayerModel : ActorModel
    {
        public UnityEvent onSwing = new();

        public EquipmentFrame equipment;

        public void Awake()
        {
            base.Awake();

            fowardActionable = new PlayerFowardActionable(this);

            equipment = new EquipmentFrame(this);
        }
    }
}