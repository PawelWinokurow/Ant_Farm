using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ChangePopUp : MonoBehaviour
{
    public TextMeshPro tmp;
    private float a;

    //private float alpha;

    private void OnEnable()
    {
        PlayAnimation();
    }

    private void Update()
    {
        tmp.transform.localPosition = Vector3.forward * a * 10f;
        tmp.alpha = 1 - a * a * a * a;
    }


    private void PlayAnimation()
    {
        tmp.transform.localPosition = Vector3.zero;
        tmp.alpha = 1f;
        // DOTween.To(() => a = 0f, x => a = x, 1f, 1f).OnComplete(()=> { GameObject.Destroy(gameObject);});
    }
}
