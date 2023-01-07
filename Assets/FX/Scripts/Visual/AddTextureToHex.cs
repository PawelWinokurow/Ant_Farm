using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AddTextureToHex : MonoBehaviour
    {

        private Surface surface;
    //public Transform marker;
    private Vector3[] points= new Vector3[81];

    public List<Material> materials;

        void Awake()
        {
        surface = GetComponent<Surface>();

        }
    public void Init()
    {
        float h = 10 * surface.h;
        float w = 10 * surface.w;

        for (int z = -4; z <= 4; z++)
        {
            for (int x = -4; x <= 4; x++)
            {
                Vector3 rand = Vector3.zero;
                if(z != 0 && x != 0)
                {
                    rand =  new Vector3(Random.Range(-w / 2f, w / 2f), 0, Random.Range(-h / 2f, h / 2f));
                }
                    Vector3 pos = surface.center + new Vector3(surface.w * (x*10 + (z % 2f) / 2f), 0f, z*10 * surface.h)+rand ;
               // Instantiate(marker, pos, Quaternion.identity);
                points[x+4+(z+4)*9] = pos;
                   
            }
        }
        
        for (int i = 0; i < points.Length; i++)//shuttle
        {
            int rnd = Random.Range(0, points.Length);

            Vector3 temp = points[i];
              points[i]= points[rnd];
            points[rnd] = temp;
        }
        

        for (int i = 0; i < surface.hexagons.Length; i++)
        {
            float minDist = Vector3.Distance(surface.hexagons[i].position, points[0]);
            int minJ=0;
            for (int j = 1; j < 81; j++)
            {
                float dist = Vector3.Distance(surface.hexagons[i].position, points[j]);
                if (dist< minDist)
                {
                    minDist = dist;
                    minJ = j;
                }
            }
            surface.hexagons[i].mr.sharedMaterial = materials[minJ % materials.Count];
        }  

    }


}

