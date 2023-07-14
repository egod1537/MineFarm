using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExMatrix4x4
{
    public static readonly Matrix4x4 YZX = new Matrix4x4(
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, 0f, 0f, 0f));
    public static readonly Matrix4x4 XZY = new Matrix4x4(
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, 0f, 0f, 0f));
    public static readonly Matrix4x4 ZXY = new Matrix4x4(
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(0f, 0f, 0f, 0f));
}
