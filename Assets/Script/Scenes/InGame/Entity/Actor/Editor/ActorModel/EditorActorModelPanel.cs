using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Entity.Actor.EditorInspector
{
    public class EditorActorModelPanel
    {
        protected ActorModel script;
        public EditorActorModelPanel(ActorModel script)
        {
            this.script = script;
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnInspectorGUI()
        {

        }
    }
}

