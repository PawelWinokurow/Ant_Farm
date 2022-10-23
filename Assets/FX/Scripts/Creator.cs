using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    private Camera cam;

    public int higth = 10;
    public int width = 10;
    private GameManager gm;
    public Transform back;
    public Vector3 ld;
    public Vector3 rd;
    public Vector3 lu;
    public Vector3 ru;

    void Start()
    {
        gm = GameManager.instance;
        Camera cam = Camera.main;
        ld = cam.ScreenToWorldPoint(new Vector3(0, 0, 1f));
        ld = Intersect(Vector3.zero, Vector3.down, cam.transform.position, ld - cam.transform.position);//ищем углы экрана
        rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1f));
        rd = Intersect(Vector3.zero, Vector3.down, cam.transform.position, rd - cam.transform.position);
        lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));
        lu = Intersect(Vector3.zero, Vector3.down, cam.transform.position, lu - cam.transform.position);
        ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));
        ru = Intersect(Vector3.zero, Vector3.down, cam.transform.position, ru - cam.transform.position);

        higth =Mathf.CeilToInt((lu.z - ld.z) / (1.5f*2f));//находим количество шестиугольников в ширину и длину
        width = Mathf.CeilToInt((rd.x - ld.x) / (0.433f*8f));
        // back.transform.localScale = new Vector3((rd.x - ld.x),1f, (lu.z - ld.z));

        gm.allHex = new Hexagon[higth * width];

        Hexagon hex=null;
        int x=0;
        int z = 0;
        float n = 1;
        for (z = 0; z < higth; z++)
        {
            n = -n;
            for (x = 0; x < width; x++)
            {
           

                hex=Instantiate(gm.wallPrefab,
                    new Vector3(x * 0.433f*4f*2f+n* 0.433f*2f, 0f,  z * 1.5f*2f)+
                    new Vector3((-width + 1f) * 0.433f * 4f,0, (-higth+1f) * 1.5f),
                    Quaternion.identity, transform);//делаем начальные стены
                hex.id = z * width + x;
                gm.AddStartWall(hex);

                if (x == 0 || x == width -1 || z==0 || z == higth-1)//ищем точки по краям что-бы запускать из них монстров
                {
                    gm.sideList.Add(hex.transform.position);
                }
            }
        }

    }

    Vector3 Intersect(Vector3 planeP, Vector3 planeN, Vector3 rayP, Vector3 rayD)//пересечение прямой и плоскости
    {
        var d = Vector3.Dot(planeP, -planeN);
        var t = -(d + Vector3.Dot(rayP, planeN)) / Vector3.Dot(rayD, planeN);
        return rayP + t * rayD;
    }




}
