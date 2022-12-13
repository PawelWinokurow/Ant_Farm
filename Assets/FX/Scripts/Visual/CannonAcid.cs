using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class CannonAcid : MonoBehaviour
    {
        void Start()
        {
        
        }

        private void OnEnable()
        {
            float flip = Random.Range(0, 2);
            flip = ExtensionMethods.Remap(flip, 0, 1, -1, 1);
            float scl = Random.Range(0.4f, 1f);
            transform.localScale = new Vector3(scl* flip, scl, scl);
        }
    }
}
