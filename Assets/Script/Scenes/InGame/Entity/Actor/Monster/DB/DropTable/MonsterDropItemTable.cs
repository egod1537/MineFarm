using Minefarm.Entity.Actor.Monster.Table;
using Minefarm.Item;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Minefarm.Entity.Actor.Monster
{
    [CreateAssetMenu(
        fileName = "MonsterDropTable", 
        menuName = "Scriptable Object/Table/Monster Drop Item Table", 
        order = int.MaxValue)]
    public class MonsterDropItemTable : ScriptableObject
    {
        [Serializable]
        public class DropItemTableDictionary : SerializableDictionary<EntityID, DropItemTable> { }

        [SerializeField]
        public DropItemTableDictionary table;

        public MonsterDropItemTable()
        {
            table = new();
            foreach (EntityID id in Enum.GetValues(typeof(EntityID)))
                table.Add(id, new DropItemTable());
        }

        public List<KeyValuePair<ItemID, int>> Pick(EntityID entityID)
            => table[entityID].Pick();
    }
}