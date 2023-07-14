using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;
using TMPro;

namespace Minefarm.Entity
{
    public class EntityModel : MonoBehaviour
    {
        public UnityEvent onSpawn = new UnityEvent();
        public UnityEvent onDeath = new UnityEvent();

        /// <summary>
        /// 엔티티 종류
        /// </summary>
        public EntityID entityID;
        /// <summary>
        /// 엔티티 소속
        /// </summary>
        public EntityGroup group;
        
        /// <summary>
        /// 존재 여부
        /// </summary>
        public bool isLive;

        /// <summary>
        /// 최대 체력
        /// </summary>
        public int maxHp;
        public float maxHpPercent = 1.0f;
        /// <summary>
        /// 체력
        /// </summary>
        public int hp;

        public int GetMaxHp() => Mathf.RoundToInt(maxHpPercent * maxHp);
    }
}