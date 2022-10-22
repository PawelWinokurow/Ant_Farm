using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameManager gm;
    public bool isTest;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && isTest)
        {

            Hexagon hex = GetComponent<Hexagon>();
            Debug.Log(hex.isDig);
            gm.Build(hex.id);

        }
    }
}
