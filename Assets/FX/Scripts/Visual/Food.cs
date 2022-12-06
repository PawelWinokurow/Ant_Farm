using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Transform angl;
    public Transform foodBody;
    public float costMax { get; set; }
    public float cost { get; set; }
    void Start()
    {
        foodBody.localEulerAngles = Vector3.up * Random.Range(0, 6) * 60f;
    }

    void Update()
    {
        angl.localPosition = new Vector3(angl.localPosition.x, ExtensionMethods.RemapClamp(1f - cost / costMax, 0f, 1f, 0f, -1.6f), angl.localPosition.z);
        foodBody.gameObject.SetActive(cost > 0);

    }
}
