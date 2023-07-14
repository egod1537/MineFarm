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
        /// ����
        /// </summary>
        public int level;
        /// <summary>
        /// ����ġ
        /// </summary>
        public int exp;

        /// <summary>
        /// ���ݷ�
        /// </summary>
        public int attack;
        public float attackPercent = 1.0f;
        public int calculatedAttack { get => Mathf.RoundToInt(attack * attackPercent); }
        /// <summary>
        /// �ʴ� ���� Ƚ��
        /// </summary>
        public float attackSpeed = 1.0f;
        public float attackSpeedPercent = 1.0f; 
        public float calculatedAttackSpeed { get => attackSpeed * attackSpeedPercent; }
        /// <summary>
        /// ����
        /// </summary>
        public int defense;
        public float defensePercent = 1.0f;
        public int calculatedDefense { get => Mathf.RoundToInt(defense * defensePercent); }
        /// <summary>
        /// �����
        /// ���͸� �����ϴ� �ɼ��̴�. �÷��̾�� 0���� ����.
        /// </summary>
        public float defenseRatio;
        /// <summary>
        /// ����� ����
        /// �÷��̾ �����ϴ� �ɼ��̴�. ���ʹ� 0���� ����.
        /// </summary>
        public float durabilityNegation;

        /// <summary>
        /// ġ��Ÿ Ȯ��
        /// </summary>
        public float criticalChance;
        /// <summary>
        /// ġ��Ÿ ������
        /// </summary>
        public float criticalDamage = 2.0f;

        /// <summary>
        /// �̵� �ӵ�
        /// </summary>
        public float speed = 1.0f;
        public float speedPercent = 1.0f;
        public float calculatedSpeed { get => speed * speedPercent; }

        /// <summary>
        /// ������
        /// </summary>
        public float jumpPower = 1.0f;

        /// <summary>
        /// ������ ����
        /// </summary>
        public EquipmentFrame equips;

        /// <summary>
        /// ���� ����Ǿ� �ִ� ȿ��
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