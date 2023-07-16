using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector3Int 
{
    public static int Dot(this Vector3Int vec, Vector3Int other)
        => vec.x * other.x + vec.y * other.y + vec.z * other.z;

    public static int Sum(this Vector3Int vec) => vec.x + vec.y + vec.z;
    public static int SumAbs(this Vector3Int vec) 
        => Mathf.Abs(vec.x) + Mathf.Abs(vec.y) + Mathf.Abs(vec.z);
}
