using UnityEngine;
using UniRx;
using UnityEngine.Events;
using Minefarm.Entity.Actor.Interface;
using Minefarm.Entity.Actor.Jumpable;
using Minefarm.Entity.Actor.Moveable;
using Minefarm.Entity.Actor.Attackable;
using Minefarm.Map.Block;
using Minefarm.Entity.Actor.FowardActionable;
using Minefarm.Entity.Actor.Damageable;
using Minefarm.Entity.Bullet;
using Minefarm.Entity.Actor.Shootable;

namespace Minefarm.Entity.Actor
{
    [RequireComponent(typeof(ActorController))]
    public class ActorModel : EntityModel
    {
        public UnityEvent<Vector3> onMove = new();
        public UnityEvent onJump = new();

        public UnityEvent<EntityModel> onFowardAction = new();
        public UnityEvent<Vector3> onShoot = new();

        public UnityEvent<ActorModel, int> onAttack = new();
        public UnityEvent<ActorModel> onKillEntity = new();
        public UnityEvent<ActorModel, int, bool> onDamage = new();

        /// <summary>
        /// ����
        /// </summary>
        public int level;
        /// <summary>
        /// ����ġ
        /// </summary>
        public int exp;

        /// <summary>
        /// �ִ� ü��
        /// </summary>
        public int maxHp;
        public float maxHpPercent = 1.0f;
        public int calculatedMaxHp { get => Mathf.RoundToInt(maxHp * maxHpPercent); }

        /// <summary>
        /// ü��
        /// </summary>
        public int hp;

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
        /// ���� ��Ÿ�
        /// </summary>
        public float attackRange = 1f;
        public float attackRangePercent = 1.0f;
        public float calculatedAttackRange { get => attackRange * attackRangePercent; }

        /// <summary>
        /// ���� �� �߻�Ǵ� ź���� �ӵ�
        /// ���� ������ ��� �ִ����� �����ȴ�.
        /// </summary>
        public float bulletSpeed = 1f;
        /// <summary>
        /// ���� �� �߻�Ǵ� ź���� ����
        /// </summary>
        public BulletModelType bulletModel = BulletModelType.Melee;

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

        public IFowardActionable fowardActionable;

        public IShootable shootable;
        public IAttackable attackable;

        public IMoveable moveable;
        public IDamageable damageable;
        public IJumpable jumpable;

        public bool isGround;

        public ActorController actorController { 
            get => (ActorController) base.entityController;
        }
        public Vector3 centerPosition { get => transform.position + Vector3.up * 0.5f; }

        public void Awake()
        {
            jumpable = new ActorJumpable(this);
            moveable = new ActorMoveable(this);

            shootable = new ActorShootable(this);
            attackable = new ActorAttackable(this);
            damageable = new ActorDamageable(this);
        }

        public bool InAttackRange(ActorModel target)
        {
            Vector3 diff = target.transform.position - transform.position;
            return diff.sqrMagnitude <= attackRange * attackRange;
        }

        public int FormulateAttack(float percent = 1.0f)
        {
            int ret = calculatedAttack;
            ret = Mathf.RoundToInt(ret * percent);
            return ret;
        }

        public int FormulateDamage(ActorModel actor, int damage, out bool isCritical)
        {
            isCritical = false;
            if (Random.Range(0f, 1f) < actor.criticalChance)
            {
                damage = Mathf.RoundToInt(actor.criticalDamage * damage);
                isCritical = true;
            }

            float negation = 1.0f - actor.defenseRatio * (1 - durabilityNegation);
            damage = Mathf.RoundToInt(damage * negation);
            damage = Mathf.Max(0, damage - calculatedDefense);
            return Mathf.Max(1, damage);
        }
        public int FormulateDamage(ActorModel actor, int damage)
        {
            bool temp = false;
            return FormulateDamage(actor, damage, out temp);
        }
    }
}