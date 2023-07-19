using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Bullet
{
    public enum BulletModelType
    {
        Melee
    }
    public static class BulletDB 
    {
        const string PATH_DB = "Entity/Bullet";

        private static Dictionary<BulletModelType, GameObject> db = new();
        public static GameObject LoadBullet(BulletModelType modelType)
        {
            if (!db.ContainsKey(modelType))
                db.Add(modelType, Resources.Load($"{PATH_DB}/{modelType}") as GameObject);
            return db[modelType];
        }
    }
}