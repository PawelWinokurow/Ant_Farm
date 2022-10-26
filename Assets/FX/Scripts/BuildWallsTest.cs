using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildWallsTest : MonoBehaviour
{
    public Surface Surface;

    public void Test()
    {
        for (int i = 0; i < Surface.allHex.Length; i++)
        {
            if (Random.Range(0, 100f) < 30f)
            {
                Surface.AddWall(i);
            }
        }
    }


}
