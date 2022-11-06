using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
    public class Floating : MonoBehaviour
    {
        private float t;
        public float speed = 1f;
    public Transform angl;
        void Update()
        {
        transform.parent.rotation = angl.rotation;
            t += Time.deltaTime;
            transform.localEulerAngles = new Vector3(Mathf.Sin(t* speed)*6f, t*70f, Mathf.Sin(t* speed + Mathf.PI * 2f / 3f)*6f);//8
            transform.localPosition = Vector3.up*Mathf.Sin(t * speed + Mathf.PI * 2f / 6f)*0.3f;
    }
    }

