using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Surface surface;
    private JobScheduler scheduler;

    private GameManager gm;

    void Start()
    {
        gm = GameManager.GetInstance();
        scheduler = gm.AntJobScheduler;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                ProcessHexagon(hit);
            }
        }
    }

    public void ProcessHexagon(RaycastHit hit)
    {
        Hexagon hex = hit.transform.GetComponent<Hexagon>();
        if (hex != null && !hex.IsInSpawnArea() && !scheduler.IsJobAlreadyCreated(hex.id))
        {

            if (hex.IsDigabble())//ставим заготовку для ямы
            {
                hex.AssignDig();
                hex = surface.AssignDigHex(hex.id);//dig
            }

            if (hex.IsBuildable())//ставим заготовку для возвышенности
            {
                hex.AssignBuild();
                hex = surface.AssignBuildHex(hex.id);//build
            }
            scheduler.AssignJob(new DigJob(hex, digHex =>
            {
                if (digHex == surface.allHex[digHex.id])
                {
                    surface.Dig(digHex.id);
                }
            }));
        }
    }
}
