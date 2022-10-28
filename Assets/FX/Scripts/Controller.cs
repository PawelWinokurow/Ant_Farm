using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    public Transform marker;
    private Surface surf;
    public bool isDig;
    private Hexagon hex;

    private void Start()
    {
        surf = Surface.instance;
        Screen.orientation = ScreenOrientation.Portrait;
        isDig = true;
    }


    public void Tap(Vector3 pos)
    {
        pos = Camera.main.ScreenToWorldPoint(pos);
        hex = surf.PositionToHex(pos);
        marker.transform.position = hex.transform.position;
        if (hex.isWall)
        {
            surf.AddDig(hex);
        }
        if(hex.isGround)
        {
            surf.AddBuild(hex);
        }
    }


    public void DigButton()
    {
        isDig = true;
    }
    public void BuildButton()
    {
        isDig = false;
    }

}

