using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gm;
    public List<Hexagon> pushedList;

    void Start()
    {
        gm = GameManager.instance;
        Screen.orientation = ScreenOrientation.Portrait;
        pushedList = new List<Hexagon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000f, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                Hexagon hex = hit.transform.GetComponent<Hexagon>();
                if (hex != null && !pushedList.Contains(hex))
                {
     
                    if (hex.isWall && !hex.isDig)//ставим заготовку для ямы
                    {
                        hex = gm.TapWall(hex);//dig
                    }

                    if (hex.isGround && !hex.isSpawn && !hex.isBuild && !hex.isDig)//ставим заготовку для возвышенности
                    {

                        hex = gm.TapGround(hex);//build
                    }

                }
                pushedList.Add(hex);
            }
        }
        else//кода отпустили  нажатие
        {
            pushedList.Clear();
        }
    }
}
