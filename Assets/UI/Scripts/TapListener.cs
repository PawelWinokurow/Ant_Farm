using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isAction;
    public Controller controller;


    public void OnPointerDown(PointerEventData eventData)
    {
        isAction = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isAction)
        {

            controller.Tap(eventData.position);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        isAction = false;
    }


}
