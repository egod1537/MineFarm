using Minefarm.Item;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Minefarm.Entity.Actor.Monster.Table
{
    [Serializable]
    public class CountPercentageTable : PercentageTable<int> { }
    [Serializable]
    public class ItemDictionary : SerializableDictionary<ItemID, CountPercentageTable> { }

    [Serializable]
    public class DropItemTable : ItemDictionary
    {
        public List<KeyValuePair<ItemID, int>> Pick()
        {
            List<KeyValuePair<ItemID, int>> ret = new();
            foreach (var item in this)
            {
                int cnt = item.Value.Pick();
                if (cnt == 0) continue;
                ret.Add(new KeyValuePair<ItemID, int>(item.Key, cnt));
            }
            return ret;
        }
    }
}