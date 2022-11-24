using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    private FloorHexagon hex;
    public GameManager GameManager;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }


    public void Tap(Vector3 pos)
    {
        pos = Camera.main.ScreenToWorldPoint(pos);
        GameManager.ProcessTap(pos);
    }


}

