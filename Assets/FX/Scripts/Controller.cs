using UnityEngine;


public class Controller : MonoBehaviour
{

    private FloorHexagon hex;
    public GameManager GameManager;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && Input.mousePosition.y / Screen.height > 0.09f)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameManager.ProcessTap(pos);
        }
    }


}

