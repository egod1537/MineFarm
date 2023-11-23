using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TTTTT : MonoBehaviour
{
    const string PATH = "temp";
    public int s, e;
    public void Run()
    {
        for(int i=s; i <= e; i++)
        {
            Texture2D tex = Resources.Load<Texture2D>($"{PATH}/{i}");
            tex.ColorClipling(Color.black, new Color(0f, 0f, 0f, 0f));
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes($"C:/Desktop/MyUnityProjects/Assets/Resources/Temp/Ret/{i}.png", bytes);
        }
    }

    void Start()
    {
        Run();
    }

}
