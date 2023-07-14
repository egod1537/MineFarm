using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExGameObject
{
    public static void Destroy(this GameObject go)
    {
        if (Application.isPlaying) Object.Destroy(go);
        else Object.DestroyImmediate(go);
    }
}
