using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExColor 
{
    public static Color FromHexCode(this Color color, string hex)
    {
        Color ret;
        ColorUtility.TryParseHtmlString(hex, out ret);
        return color = ret;
    }
}
