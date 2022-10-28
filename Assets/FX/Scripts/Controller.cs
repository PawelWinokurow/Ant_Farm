using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    private Camera cam;
    private Vector3 pos;
    public GameManager GameManager;
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);
            GameManager.ProcessHexagon(pos);
        }
    }

}

