using Minefarm.Effect;
using Minefarm.Item.Equipment;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minefarm.Entity.Actor.Player
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerView : ActorView
    {
        VertexGradient COLOR_BAD_DAMAGE
        {
            get => new VertexGradient(
            new Color().FromHexCode("#B300FFFF"),
            new Color().FromHexCode("#B300FFFF"),
            new Color().FromHexCode("#EAA8FFFF"),
            new Color().FromHexCode("#EAA8FFFF"));
        }

        protected PlayerModel playerModel {
            get => actorModel as PlayerModel;
        }

        [Serializable]
        public class EquipmentTransform : SerializableDictionary<EquipmentType, Transform> { }
        public EquipmentTransform equipmentTransform = new EquipmentTransform();

        public void Awake()
        {
            base.Awake();

            playerModel.onSwing.AddListener(() =>
            {
                animator.Play("Swing");
            });

            playerModel.onDamage.AddListener((target, damage, isCritical) =>
            {
                EffectDamage effect = Effector.CreateDamage(transform.position, damage);

                TextMeshPro tmp = effect.body.GetComponent<TextMeshPro>();
                tmp.colorGradient = COLOR_BAD_DAMAGE;
            });
        }
    }
}

