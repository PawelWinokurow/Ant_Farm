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
        angl.localPosition = Vector3.up * ExtensionMethods.RemapClamp(1f - (float)cost / (float)costMax, 0f, 1f, 0f, -1.29f);

        foodBody.gameObject.SetActive(cost > 0);

    }
}
