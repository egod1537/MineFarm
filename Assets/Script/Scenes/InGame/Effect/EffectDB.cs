using System.Collections.Generic;
using UnityEngine;

namespace Minefarm.Effect
{
    public static class EffectDB
    {
        const string PATH_DB = "Effect";

        private static Dictionary<string, GameObject> db = new();
        private static Dictionary<int, Texture> db_crack_texture = new();

        public static GameObject LoadEffect(string path)
        {
            if (!db.ContainsKey(path))
                db.Add(path, Resources.Load($"{PATH_DB}/{path}") as GameObject);
            if (db[path] == null)
                db[path] = Resources.Load($"{PATH_DB}/{path}") as GameObject;
            return db[path];
        }

        public static GameObject LoadDamage() => LoadEffect("Damage");

        public static GameObject LoadBlockCrack() => LoadEffect("Block/Crack");

        public static Texture LoadCrackTexture(int level)
        {
            if (!db_crack_texture.ContainsKey(level))
                db_crack_texture.Add(level,
                    Resources.Load($"{PATH_DB}/Block/Crack/{level}") as Texture);
            if (db_crack_texture[level] == null)
                db_crack_texture[level] = Resources.Load($"{PATH_DB}/Block/Crack/{level}") as Texture;
            return db_crack_texture[level];
        }
    }
}

