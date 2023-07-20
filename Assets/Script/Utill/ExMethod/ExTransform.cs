using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExTransform
{
    public static Matrix4x4 ToMat(this Transform transform)
        => Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
    public static Matrix4x4 ToLocalMat(this Transform transform)
        => Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

    public static void DestroyChild(this Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
            transform.GetChild(i).gameObject.Destroy();
    }
    public static void DestroyImmediateChild(this Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
    }

    public static void SetActiveChild(this Transform transform, bool isActive)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
            transform.GetChild(i).gameObject.SetActive(isActive);
    }
}
