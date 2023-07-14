using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using Minefarm.InGame;
using Minefarm.Effect.Buff;
using Minefarm.Item;
using Minefarm.Item.Equipment;
using UnityEngine.Events;

namespace Minefarm.Entity
{
    [RequireComponent(typeof(ActorController))]
    public class ActorModel : EntityModel
    {
        public UnityEvent<Vector3> onMove = new();
        public UnityEvent onJump = new();
        public UnityEvent<ActorModel, int> onAttack = new();
        public UnityEvent<ActorModel, int> onDamage = new();
        public Subject<EntityModel> onInteractive = new();

        /// <summary>
        /// 레벨
        /// </summary>
        public int level;
        /// <summary>
        /// 경험치
        /// </summary>
        public int exp;

        /// <summary>
        /// 공격력
        /// </summary>
        public int attack;
        public float attackPercent = 1.0f;
        public int calculatedAttack { get => Mathf.RoundToInt(attack * attackPercent); }
        /// <summary>
        /// 초당 공격 횟수
        /// </summary>
        public float attackSpeed = 1.0f;
        public float attackSpeedPercent = 1.0f; 
        public float calculatedAttackSpeed { get => attackSpeed * attackSpeedPercent; }
        /// <summary>
        /// 방어력
        /// </summary>
        public int defense;
        public float defensePercent = 1.0f;
        public int calculatedDefense { get => Mathf.RoundToInt(defense * defensePercent); }
        /// <summary>
        /// 방어율
        /// 몬스터만 존재하는 옵션이다. 플레이어는 0으로 고정.
        /// </summary>
        public float defenseRatio;
        /// <summary>
        /// 방어율 무시
        /// 플레이어만 존재하는 옵션이다. 몬스터는 0으로 고정.
        /// </summary>
        public float durabilityNegation;

        /// <summary>
        /// 치명타 확률
        /// </summary>
        public float criticalChance;
        /// <summary>
        /// 치명타 데미지
        /// </summary>
        public float criticalDamage = 2.0f;

        /// <summary>
        /// 이동 속도
        /// </summary>
        public float speed = 1.0f;
        public float speedPercent = 1.0f;
        public float calculatedSpeed { get => speed * speedPercent; }

        /// <summary>
        /// 점프력
        /// </summary>
        public float jumpPower = 1.0f;

        /// <summary>
        /// 장착한 장비들
        /// </summary>
        public EquipmentFrame equips;

        /// <summary>
        /// 현재 적용되어 있는 효과
        /// </summary>
        public PriorityQueue<Buff> buffs = new PriorityQueue<Buff>();

        public Transform body;

        private ActorController _controller;
        public ActorController controller { get => _controller ??= GetComponent<ActorController>(); }

        public bool isGround;

        public virtual void Update()
        {
            ProcessEquipment();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Block"))
                isGround = true;
        }

        private void ProcessEquipment()
        {
            List<EquipmentModel> models = equips.ToList();
            foreach (var model in models) model?.Update();
        }

        public void AddBuff(Buff buff)
        {
            buff.startTime = GameManager.time;
            buff.owner = this;
            buff.OnEnable();
            buffs.Add(buff);
        }

        public void ClearBuff()
        {
            foreach (Buff buff in buffs) buff.OnDisable();
            buffs.Clear();
        }

        public bool InAttackRange(ActorModel actor)
        {
            Vector3 diff = actor.transform.position - transform.position;
            float weaponRange = equips.weapon.range;
            return diff.sqrMagnitude <= weaponRange * weaponRange;
        }

        public int FormulateAttack(float percent = 1.0f)
        {
            int ret = calculatedAttack;
            ret = Mathf.RoundToInt(ret * percent);
            if (Random.Range(0f, 1f) < criticalChance)
                ret = Mathf.RoundToInt(criticalDamage * ret);
            return ret;
        }

        public int FormulateDamage(ActorModel actor, int damage)
        {
            float negation = 1.0f - actor.defenseRatio * (1 - durabilityNegation);
            damage = Mathf.RoundToInt(damage * negation);
            damage = Mathf.Max(0, damage - calculatedDefense);
            return damage;
        }
    }
}