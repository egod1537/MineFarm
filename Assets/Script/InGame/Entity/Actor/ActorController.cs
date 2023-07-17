using Minefarm.Effect.Buff;
using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Minefarm.InGame;
using Unity.VisualScripting;
using Minefarm.Item.Equipment;
using Minefarm.Map.Block;

namespace Minefarm.Entity
{
    [RequireComponent(typeof(ActorModel))]
    public class ActorController : EntityController
    {
        const float SPEED_CONSTANT = 5f;
        const float SPEED_ROTATION = 30f;

        const float JUMPPOWER_CONSTANT = 250f;
        const float DELAY_JUMP = 0.5f;
        const float DISTANCE_JUMP_CHECK = 0.5f;

        int LAYER_BLOCK { get => 1 << LayerMask.NameToLayer("Block"); }
        
        protected ActorModel actorModel { get => (ActorModel)entityModel; }

        private Rigidbody _rigidbody;
        protected Rigidbody rigidbody { get => _rigidbody ??= GetComponent<Rigidbody>(); }
        float delayJump;

        public override void Awake()
        {
            this.UpdateAsObservable()
                .Where(_ => actorModel.buffs.Count > 0 &&
                    actorModel.buffs.Peek().GetEndTime() >= GameManager.time)
                .Subscribe(_ => actorModel.buffs.Dequeue().OnDisable());

            this.UpdateAsObservable()
                .Subscribe(_ => delayJump -= Time.deltaTime);
            base.Awake();
        }

        public virtual void OnEnable()
        {
            Spawn();
        }

        public override void Spawn()
        {
            actorModel.equips = new EquipmentFrame(actorModel);
            base.Spawn();
        }

        public override void Death()
        {
            actorModel.ClearBuff();
            base.Death();
        }

        public virtual bool Move(Vector3 dir)
        {
            LookAt(dir, SPEED_ROTATION * Time.deltaTime);
            if (!IsMovement(dir)) return false;
            rigidbody.MovePosition(
                rigidbody.position + 
                dir * Time.deltaTime * actorModel.speed * SPEED_CONSTANT);
            actorModel.onMove.Invoke(dir);
            return true;
        }

        public void LookAt(Vector3 dir, float maxRadianDelta)
        {
            Vector3 rotationDir = Vector3.RotateTowards(
                actorModel.body.forward,
                dir,
                maxRadianDelta,
                0f);
            rotationDir.y = 0f;
            rigidbody.MoveRotation(Quaternion.LookRotation(rotationDir));
        }

        protected bool IsMovement(Vector3 dir)
            => GetFowardEntity(dir, Time.deltaTime * actorModel.speed * SPEED_CONSTANT, LAYER_BLOCK) == null;

        public virtual bool Jump()
        {
            if (!IsJump()) return false;
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0f;
            rigidbody.velocity = velocity;

            rigidbody.AddForce(
                transform.up * actorModel.jumpPower * JUMPPOWER_CONSTANT);
            actorModel.onJump.Invoke();

            delayJump = DELAY_JUMP;
            actorModel.isGround = false;

            return true;
        }

        protected bool IsJump()
            => delayJump <= 0f && actorModel.isGround;

        protected EntityModel GetFowardEntity(Vector3 foawrd, float distance=1f, int layer=0)
        {
            if (layer == 0) layer = ~0;
            RaycastHit hit;
            Ray ray = new Ray(rigidbody.position + transform.up * 0.5f, foawrd);

            if(Physics.SphereCast(ray, 0.25f, out hit, distance+0.5f, layer))
                return hit.transform.GetComponent<EntityModel>();
            return null;
        }
        protected EntityModel GetFowardEntity(float distance = 1f, int layer = 0)
            => GetFowardEntity(actorModel.body.forward, distance, layer);

        public virtual bool Interactive()
        {
            EntityModel entity = GetFowardEntity();
            actorModel.onInteractive.OnNext(entity);
            return entity != null;
        }

        public bool Attack(ActorModel target, int damage)
        {
            if (!actorModel.InAttackRange(target)) return false;
            if(target.controller.Damage(actorModel, damage))
            {
                actorModel.onAttack.Invoke(target, damage);
                return true;
            }
            return false;
        }
        public bool Damage(ActorModel suspect, int damage)
        {
            damage = actorModel.FormulateDamage(suspect, damage);
            if(damage > 0)
            {
                actorModel.hp -= damage;
                actorModel.onDamage.Invoke(suspect, damage);
                return true;
            }
            return false;
        }
    }
}
