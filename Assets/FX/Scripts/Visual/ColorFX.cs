using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class ColorFX : MonoBehaviour
    {
        public ParticleSystemRenderer[] renders;
        // Start is called before the first frame update
        void Start()
        {
        
        }
       public void Colorize(Color col)
        {
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material.SetColor("_Color", col);
            }
        }

    }
}
