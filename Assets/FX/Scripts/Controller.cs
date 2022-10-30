using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    public Transform marker;
    private Hexagon hex;
    public GameManager GameManager;
    public Surface Surface;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }


    public void Tap(Vector3 pos)
    {
        pos = Camera.main.ScreenToWorldPoint(pos);
        hex = Surface.PositionToHex(pos);
        marker.transform.position = hex.transform.position;
        if (hex.HexType == HEX_TYPE.EMPTY)
        {
            Surface.Fill(hex);
        }
        else
        {
            Surface.Dig(hex);
        }
        GameManager.ProcessTap(pos);
    }


}

