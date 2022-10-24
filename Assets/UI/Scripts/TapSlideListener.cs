using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapSlideListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
   // public Transform testMarker;
    private bool isButtonDown;
    private Vector3 pos;

    public void OnPointerDown(PointerEventData eventData)
    {
        pos= eventData.position;
        isButtonDown = true;
  
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        pos = eventData.position;

    }

    private void Update()
    {
      if(isButtonDown)
        {

                Ray ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
                {
                  //  testMarker.transform.position = hit.transform.position;
                }
            }
    }

}
