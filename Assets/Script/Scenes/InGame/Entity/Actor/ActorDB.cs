using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor
{
    public static class ActorDB 
    {
        const string PATH_DB = "Entity/Actor";

        private static Dictionary<EntityID, GameObject> cache = new();

        public static GameObject GetActorModel(EntityID actorID)
        {
            if (!cache.ContainsKey(actorID))
                cache.Add(actorID, Resources.Load($"{PATH_DB}/{actorID}") as GameObject);
            if (cache[actorID] == null)
                cache[actorID] = Resources.Load($"{PATH_DB}/{actorID}") as GameObject;
            return cache[actorID];
        }
    }
}

