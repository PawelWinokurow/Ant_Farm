using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    private Camera cam;

    public int higth = 10;
    public int width = 10;
   // public Hexagon hexagonPrefab;
    private GameManager gm;
    public Transform back;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        Camera cam = Camera.main;
        Vector3 ld = cam.ScreenToWorldPoint(new Vector3(0, 0, 1f));
        ld = Intersect(Vector3.zero, Vector3.down, cam.transform.position, ld - cam.transform.position);
        Vector3 rd = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1f));
        rd = Intersect(Vector3.zero, Vector3.down, cam.transform.position, rd - cam.transform.position);
        Vector3 lu = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 1f));
        lu = Intersect(Vector3.zero, Vector3.down, cam.transform.position, lu - cam.transform.position);
        Vector3 ru = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));
        ru = Intersect(Vector3.zero, Vector3.down, cam.transform.position, ru - cam.transform.position);

        higth =Mathf.CeilToInt((lu.z - ld.z) / (1.5f*2f));
        width = Mathf.CeilToInt((rd.x - ld.x) / (0.433f*8f));
       // back.transform.localScale = new Vector3((rd.x - ld.x),1f, (lu.z - ld.z));


        gm.hexagons=new List<Hexagon>();
        int x=0;
        int z = 0;
        float n = 1;
        for (z = 0; z < higth; z++)
        {
            n = -n;
            for (x = 0; x < width; x++)
            {

                gm.hexagons.Add(Instantiate(gm.hexagonWall, transform.position + new Vector3(x * 0.433f*4f*2f+n* 0.433f*2f, 0f,  z * 1.5f*2f), transform.rotation, transform));
            }
        }
        transform.position = new Vector3(-gm.hexagons[gm.hexagons.Count-1].mr.transform.position.x / 2f, 0f, -gm.hexagons[gm.hexagons.Count-1].mr.transform.position.z / 2f);
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
