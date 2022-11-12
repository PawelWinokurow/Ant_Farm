using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class SoldierAnimator : MonoBehaviour
    {
        private AnimationsScriptableObject current;
        public AnimationsScriptableObject idle;
        public AnimationsScriptableObject run;

        private int f;
        public MeshFilter mf;
        public MeshRenderer mr;

        private void Start()
        {
            current = run;
        }

        public void Run()
        {
            current = run;
            mr.materials = current.materials;
        }

        public void Idle()
        {
            current = idle;
            mr.materials = current.materials;
        }


        void Update()
        {
            f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
            mf.mesh = current.sequence[f];
        }

    }
}
