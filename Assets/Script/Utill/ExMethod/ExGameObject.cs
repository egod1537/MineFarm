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

    public static void SetActiveChild(this GameObject go, bool isActive)
    {
        int childCount = go.transform.childCount;
        for (int i = 0; i < childCount; i++)
            go.transform.GetChild(i).gameObject.SetActive(isActive);
    }
}
