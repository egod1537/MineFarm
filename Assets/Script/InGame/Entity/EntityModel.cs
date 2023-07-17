using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;
using TMPro;
using Minefarm.Map.Block;

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

        public Vector3Int mapPos
        {
            get
            {
                RaycastHit hit;
                if (Physics.SphereCast(
                    new Ray(transform.position+transform.up*0.5f, -transform.up),
                    0.25f,
                    out hit,
                    Mathf.Infinity,
                    1 << LayerMask.NameToLayer("Block")))
                    return hit.transform.GetComponent<BlockModel>().pos;
                return -Vector3Int.one;
            }
        }

        public int GetMaxHp() => Mathf.RoundToInt(maxHpPercent * maxHp);
    }
}