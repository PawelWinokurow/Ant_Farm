using System;
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
    void FixedUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
           gm.pushedList.Clear();
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000f, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                Hexagon hex = hit.transform.GetComponent<Hexagon>();
               int id = hex.id;
                if (hex != null && !gm.pushedList.Contains(id) && !hex.isSpawn)
                {

                    if (hex.isWall && !hex.isGround && !hex.isDig && !hex.isBuild)//ставим заготовку для ямы
                    {
                        hex.isDig = true;
                        hex = gm.TapToDig(hex.id);//dig
                        gm.pushedList.Add(id);
                    }

                    if (hex.isGround && !hex.isWall && !hex.isDig && !hex.isBuild)//ставим заготовку для возвышенности
                    {
                        hex.isBuild = true;
                        hex = gm.TapToBuild(hex.id);//build
                        gm.pushedList.Add(id);

                    }
                }
            }
        }


       /*
        void InstantiateAnts()
        {
            IAnt ant = Instantiate(gm.antDiggerPrefab, gm.spawnList[UnityEngine.Random.Range(0, gm.spawnList.Count)].transform.position, Quaternion.identity).GetComponent<IAnt>();

            gm.antsList.Add(ant);

            ant = Instantiate(gm.antBuilderPrefab, gm.spawnList[UnityEngine.Random.Range(0, gm.spawnList.Count)].transform.position, Quaternion.identity).GetComponent<IAnt>();
            gm.antsList.Add(ant);
        }
        */
    }
}
