using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIconGrow : MonoBehaviour
{
    public Transform tower;

    void Start()
    {
        
    }


    void Update()
    {
       float scl = ExtensionMethods.Remap(transform.localScale.x, 0.4f, 1f, 1.5f, 1f);
        tower.localScale = Vector3.one * scl;
    }
}
