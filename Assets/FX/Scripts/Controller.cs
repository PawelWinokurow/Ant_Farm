using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool onSlider;
    private bool isDrag;

    private FloorHexagon hex;
    public GameManager GameManager;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
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
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDrag && onSlider)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameManager.ProcessTap(pos);
        }
    }

}

