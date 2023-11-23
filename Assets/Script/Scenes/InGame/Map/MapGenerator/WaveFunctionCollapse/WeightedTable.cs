using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Minefarm.Map.Generator.WaveFunctionCollapse
{
    public class WeightedTable<T> : Dictionary<T, float>
    {
        public void Put(T key, float value=1.0f)
        {
            if (!this.ContainsKey(key))
                this.Add(key, 0);
            this[key] += value;
        }
        public T Pick()
        {
            float t = this.Values.Sum();
            t = Random.Range(0.0f, t);
            foreach(var pair in this)
            {
                if (t <= pair.Value)
                    return pair.Key;
                t -= pair.Value;
            }
            return default(T);
        }
        public void Normalize()
        {
            float sum = base.Values.Sum();
            var keys = base.Keys.ToList();
            foreach (var key in keys)
                base[key] /= sum;
        }
        public void Arrange()
        {
            var removeList = this.Where(pair => Mathf.Approximately(0.0f, pair.Value))
                .Select(pair => pair.Key)
                .ToList();
            foreach (var key in removeList)
                base.Remove(key);
        }

        public static WeightedTable<T> operator*(WeightedTable<T> x, WeightedTable<T> y)
        {
            WeightedTable<T> ret = new();
            foreach(var pair in x)
            {
                if (y.ContainsKey(pair.Key))
                    ret.Put(pair.Key, pair.Value * y[pair.Key]);
            }
            return ret;
        }

        public WeightedTable<T> Clone()
        {
            WeightedTable<T> ret = new();
            foreach (var pair in this)
                ret.Put(pair.Key, pair.Value);
            return ret;
        }
    }
}

