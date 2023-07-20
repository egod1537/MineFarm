using Minefarm.Entity.Actor.Block;
using Minefarm.Map.Block;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Interface
{
    public interface IDigable
    {
        public bool Dig(BlockModel target, int damage);
        public bool CanDig(BlockModel target);
    }
}
