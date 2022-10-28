using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnClick : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent Click;
    private Button button;


    private void Start()
    {
        button = GetComponent<Button>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Click.Invoke();
        //Debug.Log("Click");
    }

 

}
