using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//namespace AntFarm
//{
public class ScrollRectSnap : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public List<RectTransform> elements = new List<RectTransform>();
    private bool onSlider;
    private bool isDrag;
    private float a;
    private float a2;
    private float touchStartX;
    private float aR;
    public int n;
    private float dist;
    public RectTransform selectedField;
    private void Start()
    {
        selectedField.sizeDelta = new Vector2(selectedField.rect.height, 0);
        dist = elements[0].rect.height;
        for (int i = 0; i < elements.Count; i++)
        {
           elements[i].sizeDelta=new Vector2(dist, 0);

            float x = (Mathf.Round((a + i * dist) / dist)) * dist;
            x = x % (elements.Count * dist);
            elements[i].anchoredPosition = new Vector2(x, elements[i].anchoredPosition.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onSlider = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onSlider = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        if (onSlider)
        {
            touchStartX = eventData.pointerCurrentRaycast.screenPosition.x;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (onSlider && isDrag)
        {
            Debug.Log(transform.lossyScale);
            a += (eventData.pointerCurrentRaycast.screenPosition.x - touchStartX)* 720f/ Screen.width;
            touchStartX = eventData.pointerCurrentRaycast.screenPosition.x;
            
            if (a < 0)
            {
                a += (elements.Count * dist);
            }
            
            aR = (Mathf.Round(a / dist)) * dist;
            if (aR < 0)
            {
                aR += (elements.Count * dist);
            }

            for (int i = 0; i < elements.Count; i++)
            {
              
                    float x = a + i * dist;
                    x = x % (elements.Count * dist);
                    x -= Mathf.Round(elements.Count / 2f) * dist;
                    elements[i].anchoredPosition  = new Vector2(x, elements[i].anchoredPosition.y);

            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDrag)
        {
            a2 = a + (720 / 2 - (eventData.pointerCurrentRaycast.screenPosition.x) * 720f / Screen.width);
            aR = (Mathf.Round(a2 / dist)) * dist;
        }
    }


    // Update is called once per frame
    void Update()
    {
       
        if (!isDrag)
        {
          float  distMin = Mathf.Abs(elements[0].anchoredPosition.x);
            n = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                if(Mathf.Abs(elements[i].anchoredPosition.x)< distMin)
                {
                    distMin = Mathf.Abs(elements[i].anchoredPosition.x);
                    n = i;
                }

                a = Mathf.Lerp(a, aR, Time.deltaTime * 10f);
                float x = a + i * dist;
                x = x % (elements.Count * dist);
                if (x < 0)
                {
                    x += elements.Count * dist;
                }
                x -= Mathf.Round(elements.Count / 2f) * dist;
                elements[i].anchoredPosition = new Vector2(x, elements[i].anchoredPosition.y);
            }
        }

    }
}
//}
