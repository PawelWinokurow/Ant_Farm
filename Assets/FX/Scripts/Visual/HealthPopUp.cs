using UnityEngine;
using TMPro;
using DG.Tweening;

public class HealthPopUp : MonoBehaviour
{
    public TextMeshPro tmp;
    private float a;



    private void Update()
    {
        tmp.transform.localPosition = Vector3.forward * a * 5f;
        tmp.alpha = ExtensionMethods.RemapClamp(a, 0.8f, 1f, 1f, 0f);
    }

    public void Hit(float damage)
    {
        PlayAnimation();
        tmp.text = "-" + damage;
    }

    private void PlayAnimation()
    {
        tmp.transform.localPosition = Vector3.zero;
        tmp.alpha = 1f;
        DOTween.To(() => a = 0f, x => a = x, 1f, 1f).SetEase(Ease.Linear).OnComplete(() => { GameObject.Destroy(gameObject); });//GameObject.Destroy(gameObject);
    }
}
