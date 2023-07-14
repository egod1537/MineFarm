using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector4 
{
    public static Vector3 ToVector3(this Vector4 vec)
        => new Vector3(vec.x, vec.y, vec.z);
    public static Vector3Int ToVector3Int(this Vector4 vec)
    => new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
}
