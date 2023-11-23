using System.IO;
using UnityEngine;

namespace Minefarm.ItemSet
{
    [ExecuteInEditMode]
    public class ItemIconCapture : MonoBehaviour
    {
        public Camera camera;       //보여지는 카메라.

        public int resWidth;
        public int resHeight;
        public int cutWidth;
        public int cutHeight;
        public string savePath;
        public string filename;

        RectTransform rectTransform;
        string totalPath { get => $"{Application.dataPath}/{savePath}/{filename}.png"; }

        public void OnEnable()
        {
            rectTransform= GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.sizeDelta = new Vector2(cutWidth, cutHeight);
        }

        public void Capture()
        {
            string name = totalPath;
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(cutWidth, cutHeight, TextureFormat.RGBA32, false);
            camera.Render();
            RenderTexture.active = rt;
            Vector2 pos = rectTransform.anchoredPosition;
            screenShot.ReadPixels(new Rect(pos.x, pos.y, cutWidth, cutHeight), 0, 0);
            Color col = screenShot.GetPixel(0, 0);
            screenShot.ColorClipling(col, new Color(0f, 0f, 0f, 0f));
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(name, bytes);
        }
    }
}