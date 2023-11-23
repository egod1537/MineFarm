using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class PercentageTable<T> : SerializableDictionary<T, float>
{
    public T Pick()
    {
        float num = UnityEngine.Random.Range(0.0f, 1.0f);
        foreach(var pair in this)
        {
            T now = pair.Key;
            float weight = pair.Value;
            if (num <= weight) return now;
            num -= weight;
        }
        return default(T);
    }
    
    public void Normalize()
    {
        float sum = 0.0f;
        foreach (var pair in this) sum += pair.Value;
        if (Mathf.Approximately(sum, 0.0f)) return;
        List<T> keys = new(Keys);
        foreach (var key in keys) this[key] /= sum;
    }
}
