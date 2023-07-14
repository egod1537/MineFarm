using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector3Int 
{
    public static int Dot(this Vector3Int vec, Vector3Int other)
        => vec.x * other.x + vec.y * other.y + vec.z * other.z;
}
