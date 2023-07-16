using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item.Equipment
{
    public class EquipmentModel : ItemModel
    {
        /// <summary>
        /// 공격력
        /// </summary>
        public int plusAttack;
        public float plusAttackPercent = 1.0f;

        /// <summary>
        /// 사거리
        /// </summary>
        public float range;

        /// <summary>
        /// 초당 공격 횟수
        /// </summary>
        public float plusAttackSpeed;
        public float plusAttackSpeedPercent = 1.0f;
        /// <summary>
        /// 방어력
        /// </summary>
        public int plusDefense;
        public float plusDefensePercent = 1.0f;

        /// <summary>
        /// 치명타 확률
        /// </summary>
        public float plusCriticalChance;
        /// <summary>
        /// 치명타 데미지
        /// </summary>
        public float plusCriticalDamage;

        /// <summary>
        /// 이동 속도
        /// </summary>
        public float plusSpeed;
        public float plusSpeedPercent = 1.0f;

        /// <summary>
        /// Entity가 weapon에 들고 Interactive할 때 호출되는 함수
        /// </summary>
        public void OnInteractive()
        {

        }

        public void OnEquip()
        {

        }

        public void OnUnEquip()
        {

        }

        public void OnUse()
        {

        }

        public void OnPickUp()
        {

        }

        public void OnDrop()
        {

        }

        public void Update()
        {
            
        }
    }
}

