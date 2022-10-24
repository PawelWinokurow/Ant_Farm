using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnClick : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent Click;

    public void OnPointerDown(PointerEventData eventData)
    {
        Click.Invoke();
        Debug.Log("Click");
    }
}
