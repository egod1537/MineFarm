using Minefarm.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Item.Equipment
{
    public class EquipmentFrame
    {
        /// <summary>
        /// 소유자
        /// </summary>
        public ActorModel owner { get; private set; }

        /// <summary>
        /// 무기
        /// </summary>
        public EquipmentModel weapon { get; private set; }

        /// <summary>
        /// 모자
        /// </summary>
        public EquipmentModel helment { get; private set; }
        /// <summary>
        /// 흉갑
        /// </summary>
        public EquipmentModel chestPlate { get; private set; }
        /// <summary>
        /// 바지
        /// </summary>
        public EquipmentModel leggins { get; private set; }
        /// <summary>
        /// 신발
        /// </summary>
        public EquipmentModel shoes { get; private set; }

        /// <summary>
        /// 반지1
        /// </summary>
        public EquipmentModel ring1 { get; private set; }
        /// <summary>
        /// 반지2
        /// </summary>
        public EquipmentModel ring2 { get; private set; }

        public EquipmentFrame(ActorModel actor)
        {
            this.owner = actor;
        }

        public EquipmentFrame EquipWeapon(EquipmentModel shoes)
        {
            return this;
        }
        public EquipmentFrame EquipHelmet(EquipmentModel shoes)
        {
            return this;
        }
        public EquipmentFrame EquipChestPlate(EquipmentModel shoes)
        {
            return this;
        }
        public EquipmentFrame EquipLeggins(EquipmentModel shoes)
        {
            return this;
        }
        public EquipmentFrame EquipShoes(EquipmentModel shoes)
        {
            return this;
        }
        public EquipmentFrame EquipRing(int idx, EquipmentModel ring)
        {
            return this;
        }

        public List<EquipmentModel> ToList()
            => new List<EquipmentModel>()
            {weapon, helment, chestPlate, leggins, shoes, ring1, ring2};
    }
}
