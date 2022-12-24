using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Capture();
        }
    }

    public void Capture()
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 1000000, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        RenderTexture oldRT = camera.targetTexture;
        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = oldRT;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, true);

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string path = "Assets/screenshot.png";
        System.IO.File.WriteAllBytes(path, bytes);
       // UnityEditor.AssetDatabase.ImportAsset(path);
    }
}

