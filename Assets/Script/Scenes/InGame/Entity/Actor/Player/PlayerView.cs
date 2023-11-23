using Minefarm.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Minefarm.Item.Equipment;
using Minefarm.Effect.InGame;
using Minefarm.Item;
using Minefarm.Inventory.UI;

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

        public Transform handleTransform;

        private EffectSkinnedMeshAfterImage[] effectAfterImage;

        public void Awake()
        {
            base.Awake();

            SubscribeDamage();
            SubscribeDig();
            SubscribeDash();
        }

        public void Start()
        {
            SubscribeHandleItem();
        }

        private void SubscribeDig()
        {
            playerModel.onDig.AddListener((block, damage) =>
            {
                animator.Play("Dig");
            });
        }
        private void SubscribeDash()
        {
            effectAfterImage = GetComponentsInChildren<EffectSkinnedMeshAfterImage>();
            foreach (var vfx in effectAfterImage) vfx.enabled = false;

            playerModel.onDash.AddListener(() =>
            {
                foreach (var vfx in effectAfterImage) vfx.enabled = true;
            });
            playerModel.onDashEnd.AddListener(() =>
            {
                foreach (var vfx in effectAfterImage) vfx.enabled = false;
            });
        }
        private void SubscribeDamage()
        {
            playerModel.onDamage.AddListener((target, damage, isCritical) =>
            {
                EffectDamage effect = Effector.CreateDamage(transform.position, damage);

                TextMeshPro tmp = effect.body.GetComponent<TextMeshPro>();
                tmp.colorGradient = COLOR_BAD_DAMAGE;
            });
        }
        private void SubscribeHandleItem()
        {
            void UpdateHandleItem(ItemModel item)
            {
                handleTransform.DestroyChild();

                if (item == null) return;

                GameObject go = Instantiate(ItemDB.GetItemModel(item.itemID));

                Transform tr = go.transform;
                tr.SetParent(handleTransform);
                tr.localPosition = Vector3.zero;
                tr.localEulerAngles = Vector3.zero;
                tr.localScale = Vector3.one;
            }

            playerModel.onSetHandleItem.AddListener(item => UpdateHandleItem(item));
        }
    }
}

