using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExList
{
    public static T Back<T>(this List<T> list) => list[list.Count - 1];
}
