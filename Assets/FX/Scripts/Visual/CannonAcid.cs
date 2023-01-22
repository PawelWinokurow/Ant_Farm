using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class CannonAcid : MonoBehaviour
    {
        private float startScl;
        void Awake()
        {
            startScl = transform.localScale.x;
            Debug.Log(startScl);
        }

        private void OnEnable()
        {
            float flip = Random.Range(0, 2);
            flip = ExtensionMethods.Remap(flip, 0, 1, -1, 1);
            float scl = startScl* Random.Range(0.7f, 1f);
            transform.localScale = new Vector3(scl* flip, scl, scl);
        }
   
    }
}
