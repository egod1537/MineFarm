using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector3
{
    public static Vector3Int ToVector3Int(this Vector3 vec)
        => new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);

    public static Vector3 ToRound(this Vector3 vec)
        => new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));

    public static Vector3 Clamp(this Vector3 vec, Vector3 start, Vector3 end)
    {
        Vector3 ret = vec;
        ret.x = Mathf.Clamp(ret.x, start.x, end.x);
        ret.y = Mathf.Clamp(ret.y, start.y, end.y);
        ret.z = Mathf.Clamp(ret.z, start.z, end.z);
        return ret;
    }
}
