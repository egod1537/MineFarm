using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Effect
{
    public static class EffectDB
    {
        const string PATH_DB = "Effect";
        private static Dictionary<string, GameObject> cache = new();

        public static GameObject LoadEffect(string path)
        {
            if (!cache.ContainsKey(path))
                cache.Add(path, Resources.Load($"{PATH_DB}/{path}") as GameObject);
            return cache[path];
        }

        public static GameObject LoadDamage() => LoadEffect("Damage");
    }
}

