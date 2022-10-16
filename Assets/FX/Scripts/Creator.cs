using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    private Camera cam;

    public int higth = 10;
    public int width = 10;
    public Hexagon hexagonPrefab;
    private GameManager gm;
    public Transform back;
    public GameObject marker;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        Camera cam = Camera.main;
        Vector3 ld = cam.ScreenToWorldPoint(new Vector3(0, 0, 1f));
        ld = Intersect(Vector3.zero, Vector3.back, cam.transform.position, ld - cam.transform.position);
        Vector3 rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1f));
        rd = Intersect(Vector3.zero, Vector3.back, cam.transform.position, rd - cam.transform.position);
        Vector3 lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));
        lu = Intersect(Vector3.zero, Vector3.back, cam.transform.position, lu - cam.transform.position);
        Vector3 ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));
        ru = Intersect(Vector3.zero, Vector3.back, cam.transform.position, ru - cam.transform.position);
        Instantiate(marker, ru, cam.transform.rotation);


        higth =Mathf.CeilToInt((lu.y - ld.y) / 0.433f);
        width = Mathf.CeilToInt((rd.x - ld.x) / 1.5f);
        back.transform.localScale = new Vector3((rd.x - ld.x),(lu.y - ld.y), 1f);


        gm.hexagons = new Hexagon[width, higth];
        int x=0;
        int y = 0;
        float n = 1;
        for (y = 0; y < higth; y++)
        {
            n = -n;
            for (x = 0; x < width; x++)
            {
                gm.hexagons[x,y]= Instantiate(hexagonPrefab, transform.position + new Vector3(x * 1.5f + n*1.5f / 4f, y * 0.433f, 0f), transform.rotation, transform);
            }
        }
        transform.position = new Vector3(-gm.hexagons[x-1, y-1].mr.transform.position.x / 2f, -gm.hexagons[x-1, y-1].mr.transform.position.y / 2f, 0f);
    }

    Vector3 Intersect(Vector3 planeP, Vector3 planeN, Vector3 rayP, Vector3 rayD)
    {
        var d = Vector3.Dot(planeP, -planeN);
        var t = -(d + Vector3.Dot(rayP, planeN)) / Vector3.Dot(rayD, planeN);
        return rayP + t * rayD;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
