using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gm;


    void Start()
    {
        gm = GameManager.instance;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Hexagon hex = hit.transform.gameObject.GetComponent<Hexagon>();
                if (hex != null)
                {
                    if (hex.isWall && !hex.isDig && !hex.isGround && !hex.isSpawn)//строим яму
                    {
                        hex.isWall = false;
                        hex.isDig = true;
                        hex.isGround = false;
                        gm.TapWall(hex);

                    }

                    if (!hex.isWall && !hex.isDig && hex.isGround && !hex.isSpawn)//строим возвышенность
                    {
                        Hexagon wall = gm.TapGround(hex);
                        wall.isWall = true;
                        wall.isDig = false;
                        wall.isGround = true;
                    }

                }
            }
        }
        else
        {
            for (int i = 0; i < gm.wallList.Count; i++)
            {
                if(gm.wallList[i].isGround && gm.wallList[i].isWall)//кода отпустили
                {
                    gm.wallList[i].isGround = false;
                }    
            }
        }
    }
}
