using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapSlideListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // public Transform testMarker;
    private bool isButtonDown;
    public Transform marker;
    private Vector3 pos;
    private int x;
    private int z;
    private GameManager gm;
    private Camera cam;
    private Surface surf;
    private void Start()
    {
        surf = gm.Surface;
        cam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }



    private void Update()
    {

        if (isButtonDown)
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);
            // marker.transform.position = new Vector3(x* surf.w + (z % 2) * 0.5f* surf.w, 0, z* surf.h);
            marker.transform.position = surf.allHex[surf.PositionToId(pos)].transform.position;
        }
    }


}
