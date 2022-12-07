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
       public void Colorize(Material mat)
        {
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].material.SetColor("_Color", mat.GetColor("_Color"));
            }
        }

    }
}
