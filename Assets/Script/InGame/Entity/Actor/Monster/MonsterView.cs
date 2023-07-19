using Minefarm.Effect;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster
{
    public class MonsterView : ActorView
    {
        VertexGradient COLOR_CRITICAL_DAMAGE
        {
            get => new VertexGradient(
            new Color().FromHexCode("#FF0008FF"),
            new Color().FromHexCode("#FF0008FF"),
            new Color().FromHexCode("#FF64C3FF"),
            new Color().FromHexCode("#FF64C3FF"));
        }
        VertexGradient COLOR_NORMAL_DAMAGE
        {
            get => new VertexGradient(
            new Color().FromHexCode("#FF5C00FF"),
            new Color().FromHexCode("#FF5C00FF"),
            new Color().FromHexCode("#ECB900FF"),
            new Color().FromHexCode("#ECB900FF"));
        }

        public void Awake()
        {
            base.Awake();

            actorModel.onDamage.AddListener((target, damage, isCritical) =>
            {
                EffectDamage effect = Effector.CreateDamage(transform.position, damage);

                Effector.CreateDamage(
                    transform.position, 
                    damage, 
                    (isCritical ? COLOR_CRITICAL_DAMAGE : COLOR_NORMAL_DAMAGE));

                Vector3 dir = target.transform.position - transform.position;
                dir.y = 0;
                actorModel.body.transform.rotation = Quaternion.LookRotation(dir);
            });
        }
    }
}