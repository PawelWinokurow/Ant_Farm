using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (rayHit)
            {

                Hexagon hex = rayHit.collider.gameObject.GetComponent<Hexagon>();

                if (hex)
                {
                    hex.mr.material = mat;
                    hex.isSelected = true;
                }
            }
           // Debug.Log("Target Position: " + rayHit.collider.gameObject.transform.position);

        }
 
    }
}
