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

    public static Color SetOpacity(this Color color, float opacity)
    {
        Color ret = color;
        ret.a = opacity;
        return ret;
    }
}
