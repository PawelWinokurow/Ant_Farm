using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Agent : MonoBehaviour
    {
       // private Surface2 surf;
        private Hexagon current;
        private List<Hexagon> path;
        private float t;
        private int i;
        //public ParticleSystem puffFX;
    void Start()
        {
          //  surf = Surface2.instance;
       // current = surf.allHex[surf.PositionToId(transform.position)];
       // transform.position = current.transform.position;
        }
   public void GoTo( Hexagon goalHex)
    {
       // current = surf.allHex[surf.PositionToId(transform.position)];
       // path = PathFinder.instance.GetPath(current, goalHex);
       
        t = 0;
        i = 0;
    }

        void Update()
        {
     

            if (path!=null && i < path.Count - 1)
            {
                if (t < 1f)
                {
                    t += Time.deltaTime / 0.2f;
                    transform.position = Vector3.Lerp(path[i].transform.position, path[i + 1].transform.position, t);
                }
                else
                {     
                i++;
                 t = 0;
                if (i<(path.Count-1) && path[i+1].isWall)
                {
                   // surf.SetGround(path[i+1]);
                    //Instantiate(puffFX, path[i+1].transform.position, Quaternion.identity);
                }
                }
            }
        }


            
    }

