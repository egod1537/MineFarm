using Minefarm.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Effect.Buff
{
    public class Buff : IComparable
    {
        public ActorModel owner;

        public float duration;
        public float startTime;

        public virtual void OnEnable()
        {

        }
        
        public virtual void OnDisable()
        {

        }

        public float GetEndTime() => startTime + duration;

        public int CompareTo(object obj)
        {
            Buff other = obj as Buff;
            float at = GetEndTime(), bt = other.GetEndTime();
            if (at < bt) return -1;
            else if (at > bt) return 1;
            else return 0;
        }
    }
}