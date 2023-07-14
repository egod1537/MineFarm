using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonDB : MonoBehaviour
{
    public static string ObjectToJson(System.Object obj) => JsonUtility.ToJson(obj);
    public static T JsonToObject<T>(string json) => JsonUtility.FromJson<T>(json);

    public static void SaveJsonFile<T>(string path, T data)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            byte[] dt = Encoding.UTF8.GetBytes(ObjectToJson(data));
            fs.Write(dt, 0, dt.Length);
        }
    }

    public static T LoadJsonFile<T>(string path)
    {
        byte[] data;
        using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
        }
        string jsonData = Encoding.UTF8.GetString(data);

        return JsonToObject<T>(jsonData);
    }
}
