using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity
{
    public class ActorView : MonoBehaviour
    {
        private ActorModel _model;
        public ActorModel model { get => _model ??= GetComponent<ActorModel>(); }

        Animator animtor;

        public void Awake()
        {
            animtor = model.body.GetComponent<Animator>();

            model.onMove.AddListener(dir =>
            {
                animtor.SetFloat("Move X", dir.x);    
                animtor.SetFloat("Move Y", dir.z);    
            });

            model.onAttack.AddListener((target, damage) =>
            {
                animtor.Play("Swing");
            });
        }

        private void Update()
        {
            animtor.SetBool("IsGround", model.isGround);
        }
    }
}
