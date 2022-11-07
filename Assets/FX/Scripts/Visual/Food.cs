using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Food : MonoBehaviour
{
    public Transform angl;
    public Transform foodBody;
    public GameObject[] uiPoints;
    [Range(0, 5)]
    public int antCount;
    private int antCountOld = -1;

    public int costMax = 100;
    public int cost;
    void Start()
    {
        foodBody.localEulerAngles = Vector3.up * Random.Range(0, 6) * 60f;
        cost = costMax;
    }

    void Update()
    {
        if (antCount != antCountOld)
        {
            for (int i = 0; i < uiPoints.Length; i++)
            {
                uiPoints[i].SetActive(i < antCount);

            }
            antCountOld = antCount;
        }
        angl.localPosition = new Vector3(angl.localPosition.x, ExtensionMethods.RemapClamp(1f - (float)cost / (float)costMax, 0f, 1f, 0f, -1.6f), angl.localPosition.z);

        foodBody.gameObject.SetActive(cost > 0);

    }
}
