using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class Dig_FX : MonoBehaviour
    {
        public ParticleSystemRenderer debrisRend;
        public ParticleSystemRenderer cloundsRend;
        public ParticleSystem ps;
        public Surface surface;

        void Start()
        {
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Texture texture = surface.PositionToHex(transform.position).GetComponent<MeshRenderer>().sharedMaterial.GetTexture("_MainTex");
                debrisRend.material.SetTexture("_MainTex", texture);
                cloundsRend.material.SetTexture("_MainTex", texture);
                ps.Clear();
                ps.Play();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
               // ps.Stop();
            }
            }
    }
}
