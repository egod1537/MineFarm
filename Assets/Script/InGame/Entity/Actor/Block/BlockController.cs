using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Block
{
    public class BlockController : ActorController
    {
        private void Awake()
        {
            base.Awake();

            actorModel.onKill.AddListener(() => Destroy(gameObject));
        }
    }
}

