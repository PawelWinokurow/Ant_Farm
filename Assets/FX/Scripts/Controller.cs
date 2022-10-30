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
        if (hex.isWall)
        {
            Surface.AddDig(hex);
        }
        if (hex.isGround)
        {
            Surface.AddBuild(hex);
        }
        GameManager.ProcessTap(pos);
    }


}

