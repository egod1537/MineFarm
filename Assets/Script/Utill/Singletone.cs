using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _ins;
    public static T ins
    {
        get
        {
            if(_ins == null)
            {
                _ins ??= FindObjectOfType<T>();
                if(_ins == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).Name;
                    _ins = go.AddComponent<T>();
                }
            }
            return _ins;
        }
    }
}
