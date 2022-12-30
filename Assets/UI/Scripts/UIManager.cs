using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

    public TMP_Text money;
    public SpawnIcon[] spawnIcons;


    void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        
    }

    public void EnemySpawnIcons(int[] number)
    {
        int n = 0;
        for (int i = 0; i < spawnIcons.Length; i++)
        {
            if(number[i] > 0)
            {
                spawnIcons[i].gameObject.SetActive(true);
                spawnIcons[i].Play(n*0.2f, number[i]);
                n++;
            }
            else
            {
                spawnIcons[i].gameObject.SetActive(false);
            }


        } 

    }
    public void MoneyCounter(int n)
    {
        money.text = n.ToString();
    }

}
