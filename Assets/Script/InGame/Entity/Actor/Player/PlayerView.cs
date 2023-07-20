using Minefarm.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Minefarm.Entity.Item.Equipment;
using Minefarm.Effect.InGame;

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

        private EffectSkinnedMeshAfterImage[] effectAfterImage;

        public void Awake()
        {
            base.Awake();

            effectAfterImage = GetComponentsInChildren<EffectSkinnedMeshAfterImage>();
            foreach (var vfx in effectAfterImage) vfx.enabled = false;

            playerModel.onDamage.AddListener((target, damage, isCritical) =>
            {
                EffectDamage effect = Effector.CreateDamage(transform.position, damage);

                TextMeshPro tmp = effect.body.GetComponent<TextMeshPro>();
                tmp.colorGradient = COLOR_BAD_DAMAGE;
            });

            playerModel.onDash.AddListener(() =>
            {
                foreach (var vfx in effectAfterImage) vfx.enabled = true;
            });
            playerModel.onDashEnd.AddListener(() =>
            {
                foreach (var vfx in effectAfterImage) vfx.enabled = false;
            });
        }
    }
}

