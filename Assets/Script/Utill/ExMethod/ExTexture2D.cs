using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExTexture2D
{
    public static Texture2D ResizeTexture2D(this Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = new RenderTexture(newWidth, newHeight, 0);
        Graphics.Blit(source, rt);
        Texture2D newTexture = new Texture2D(newWidth, newHeight);
        newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTexture.Apply();
        RenderTexture.active = null;
        rt.Release();
        return newTexture;
    }

    public static Texture2D ColorClipling(this Texture2D tex, Color from, Color to)
    {
        int width = tex.width, height = tex.height;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (tex.GetPixel(i, j) == from)
                    tex.SetPixel(i, j, to);
        return tex;
    }
}
