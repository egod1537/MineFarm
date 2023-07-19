using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Effect
{
    public static class EffectDB
    {
        const string PATH_DB = "Effect";
        private static Dictionary<string, GameObject> db = new();

        public static GameObject LoadEffect(string path)
        {
            if (!db.ContainsKey(path))
                db.Add(path, Resources.Load($"{PATH_DB}/{path}") as GameObject);
            return db[path];
        }

        public static GameObject LoadDamage() => LoadEffect("Damage");
    }
}

