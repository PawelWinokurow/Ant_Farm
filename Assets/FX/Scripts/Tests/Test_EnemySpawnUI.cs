using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemySpawnUI : MonoBehaviour
{
    private int[] number = new int[4];

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if( Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 4; i++)
            {
                number[i] = Random.Range(0, 100);
            }
           
            UIManager.instance.EnemySpawnIcons(number);

            UIManager.instance.MoneyCounter(Random.Range(0, 1000));
        }
    }
}
