using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    private Camera cam;

    public int higth = 10;
    public int width = 10;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;

    public void Init(Surface surface)
    {
        cam = Camera.main;
        ld = cam.ScreenToWorldPoint(new Vector3(0, 0, 1f));
        ld = ExtensionMethods.Intersect(Vector3.zero, Vector3.down, cam.transform.position, ld - cam.transform.position);//ищем углы экрана
        rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1f));
        rd = ExtensionMethods.Intersect(Vector3.zero, Vector3.down, cam.transform.position, rd - cam.transform.position);
        lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));
        lu = ExtensionMethods.Intersect(Vector3.zero, Vector3.down, cam.transform.position, lu - cam.transform.position);
        ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));
        ru = ExtensionMethods.Intersect(Vector3.zero, Vector3.down, cam.transform.position, ru - cam.transform.position);

        higth = Mathf.CeilToInt((lu.z - ld.z) / (1.5f * 2f));//находим количество шестиугольников в ширину и длину
        width = Mathf.CeilToInt((rd.x - ld.x) / (0.433f * 8f));

        surface.allHex = new Hexagon[higth * width];

        Hexagon hex = null;
        int x = 0;
        int z = 0;
        float n = 1;
        for (z = 0; z < higth; z++)
        {
            n = -n;
            for (x = 0; x < width; x++)
            {
                hex = Instantiate(surface.wallPrefab,
                    new Vector3(x * 0.433f * 4f * 2f + n * 0.433f * 2f, 0f, z * 1.5f * 2f) +
                    new Vector3((-width + 1f) * 0.433f * 4f, 0, (-higth + 1f) * 1.5f),
                    Quaternion.identity, transform);//делаем начальные стены
                hex.id = z * width + x;
                surface.AddStartWall(hex);

                if (x == 0 || x == width - 1 || z == 0 || z == higth - 1)//ищем точки по краям что-бы запускать из них монстров
                {
                    surface.sideList.Add(hex.transform.position);
                }
            }
        }

    }


}
