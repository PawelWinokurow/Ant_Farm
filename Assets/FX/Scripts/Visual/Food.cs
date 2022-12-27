using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Transform angl;
    public Transform foodBody;
    public Transform shadow;
    public float costMax { get; set; }
    public float cost { get; set; }
    void Start()
    {
        foodBody.localEulerAngles = Vector3.up * Random.Range(0, 6) * 60f;
    }

    void Update()
    {
        Vector3 scl = Vector3.one * ExtensionMethods.RemapClamp(1f - cost / costMax, 0f, 1f, 1f, 0.4f);
        shadow.localScale = scl * 5f;
        angl.localScale = scl;
        angl.localPosition = new Vector3(angl.localPosition.x, ExtensionMethods.RemapClamp(1f - cost / costMax, 0f, 1f, 0f, -0.5f), angl.localPosition.z);
        foodBody.gameObject.SetActive(cost > 0);

    }
}
