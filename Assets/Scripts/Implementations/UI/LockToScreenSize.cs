using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockToScreenSize : MonoBehaviour
{
    [SerializeField] private bool _isLookAtCamera;
    [SerializeField] private float sizeOnScreen;
    [SerializeField] 
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 a = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 b = new Vector3(a.x, a.y + sizeOnScreen, a.z);

        Vector3 aa = Camera.main.ScreenToWorldPoint(a);
        Vector3 bb = Camera.main.ScreenToWorldPoint(b);

        transform.localScale = Vector3.one * (aa - bb).magnitude;

        //    if ( _camera )
        // {
        //    float camHeight;
        //   
        //    camHeight = _camera.orthographicSize;
        //
        //    if (_isLookAtCamera)
        //    {
        //       transform.rotation = _camera.transform.rotation;
        //    }
        //
        //    if (_camera.orthographicSize > 39f)
        //    {
        //       float scale = (camHeight / 40f) * 0.55f;
        //       transform.localScale = Vector3.one + Vector3.one * scale;
        //    }
        // } else
        // {
        //    _camera = Camera.main;
        // }
    }
}
