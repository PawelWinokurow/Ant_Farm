using UnityEngine;
using TMPro;
using DG.Tweening;

public class PopUp : MonoBehaviour
{
    public Transform body;
    public TextMeshPro tmp;
    private float a;


    private void OnEnable()
    {
        PlayAnimation();
    }


    private void PlayAnimation()
    {
        body.transform.localPosition = Vector3.zero;
        tmp.alpha = 1f;
        DOTween.To(() => a = 0f, x => a = x, 1f, 1f).SetEase(Ease.Linear).OnUpdate(() => {
            body.transform.localPosition = Vector3.forward * a * 5f;
            tmp.alpha = ExtensionMethods.RemapClamp(a, 0.8f, 1f, 1f, 0f);
        }).OnComplete(() => { GameObject.Destroy(gameObject); });//GameObject.Destroy(gameObject);
    }
}
