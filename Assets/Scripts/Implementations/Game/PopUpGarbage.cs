using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class PopUpGarbage : MonoBehaviour
{
    [SerializeField] private TextMeshPro _valueCurrents;
    private float t;
    private float l;

    //private float alpha;
    public void Init(string value)
    {
        _valueCurrents.text = "+" + value;
        PlayAnimation();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, 0f, 0f));
            mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, 1f, 0.2f).SetEase(Ease.OutCirc));
            mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, 0f, 0.2f).SetEase(Ease.InCirc).SetDelay(0.5f));

            Sequence mySequence2 = DOTween.Sequence();
            mySequence2.Append(_valueCurrents.transform.DOLocalMoveY(0f, 0f).SetEase(Ease.Linear));
            mySequence2.Append(_valueCurrents.transform.DOLocalMoveY(10f,0.9f).SetEase(Ease.Linear));

        }
    }

    private void PlayAnimation()
    {
        t = Random.Range(0.5f, 1f);
        l = Random.Range(0f, 5f);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, 0f, 0f));
        mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, Random.Range(0.7f,1f), 0.2f).SetEase(Ease.OutCirc));
        mySequence.Append(DOTween.To(() => _valueCurrents.alpha, x => _valueCurrents.alpha = x, 0f, 0.2f).SetEase(Ease.InCirc).SetDelay(t));

        Sequence mySequence2 = DOTween.Sequence();
        mySequence2.Append(_valueCurrents.transform.DOLocalMoveY(l, 0f).SetEase(Ease.Linear));
        mySequence2.Append(_valueCurrents.transform.DOLocalMoveY(l+10f, t+0.4f).SetEase(Ease.Linear));
    }
}
