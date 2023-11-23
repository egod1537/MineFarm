using Minefarm.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster
{
    public static class MonsterDB
    {
        const string PATH_DROP_TABLE = "Table/Monster Drop Table";

        private static MonsterDropItemTable dropTable;

        static MonsterDB()
        {
            dropTable = Resources.Load(PATH_DROP_TABLE) as MonsterDropItemTable;
        }

        public static List<KeyValuePair<ItemID, int>> GetDropItems(EntityID entityID)
            => dropTable.Pick(entityID);
    }
}