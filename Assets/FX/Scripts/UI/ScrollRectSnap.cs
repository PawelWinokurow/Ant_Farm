using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//namespace AntFarm
//{
public class ScrollRectSnap : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public List<RectTransform> elements = new List<RectTransform>();
    private bool onSlider;
    private bool isDrag;
    private float a;
    private float touchStartX;
    private float aR;
    public int n;

    private void Start()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            float x = (Mathf.Round((a + i * 100f) / 100f)) * 100f;
            x = x % (elements.Count * 100);
            x -= Mathf.Round(elements.Count / 2f) * 100;
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
            touchStartX = Input.mousePosition.x;
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
            a += (Input.mousePosition.x - touchStartX);
            touchStartX = Input.mousePosition.x;

            if (a < 0)
            {
                a += (elements.Count * 100);
            }

            aR = (Mathf.Round(a / 100f)) * 100f;
            if (aR < 0)
            {
                aR += (elements.Count * 100);
            }

            for (int i = 0; i < elements.Count; i++)
            {
              
                    float x = a + i * 100f;
                    x = x % (elements.Count * 100);
                    x -= Mathf.Round(elements.Count / 2f) * 100;
                    elements[i].anchoredPosition = new Vector2(x, elements[i].anchoredPosition.y);

            }

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
                float x = a + i * 100f;
                x = x % (elements.Count * 100);
                x -= Mathf.Round(elements.Count / 2f) * 100;
                elements[i].anchoredPosition = new Vector2(x, elements[i].anchoredPosition.y);
            }
        }

    }
}
//}
