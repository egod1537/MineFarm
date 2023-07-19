using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor
{
    public class ActorView : MonoBehaviour
    {
        private ActorModel _actorModel;
        public ActorModel actorModel { get => _actorModel ??= GetComponent<ActorModel>(); }

        protected Animator animator;

        public void Awake()
        {
            animator = actorModel.body.GetComponent<Animator>();

            actorModel.onMove.AddListener(dir =>
            {
                animator.SetFloat("Move X", dir.x);
                animator.SetFloat("Move Y", dir.z);    
            });

            actorModel.onShoot.AddListener(direction =>
            {
                animator.Play("Attack");
            });

            actorModel.onDamage.AddListener((target, damage, isCritical) =>
            {
                animator.Play("Damage");
            });
        }

        private void Update()
        {
            animator.SetBool("IsGround", actorModel.isGround);
        }
    }
}
