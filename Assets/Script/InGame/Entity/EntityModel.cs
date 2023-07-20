using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;
using TMPro;
using Minefarm.Map.Block;
using Minefarm.Entity.Actor;
using Minefarm.Entity.Actor.Block;

namespace Minefarm.Entity
{
    /// <summary>
    /// Map에 존재할 수 있는 객체
    ///  - 이동정보, 생성, 파괴
    /// </summary>
    public class EntityModel : MonoBehaviour
    {
        public UnityEvent onSpawn = new UnityEvent();
        public UnityEvent onKill = new UnityEvent();

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

        public Transform body;

        private EntityController _entityController;
        public EntityController entityController { 
            get => _entityController ??= GetComponent<EntityController>();
        }
    }
}