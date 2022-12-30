using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SpawnIcon : MonoBehaviour
{
    public Transform item;
    public TMP_Text tmp;
    public CanvasGroup canvasGroup;
    private Vector3 startPos;
    private Sequence sequence;

    void Start()
    {
        startPos = item.transform.localPosition;
    }

    public void Play(float delay, int count)
    {
        sequence?.Kill();
        tmp.text = "x"+ count.ToString();
        canvasGroup.alpha = 1f;
        item.localPosition = startPos;
        sequence = DOTween.Sequence();
        sequence.Append(item.DOLocalMove(Vector3.zero, 0.5f).SetDelay(delay));
        //sequence.Append(item.DOLocalMove(startPos, 0.5f).SetDelay(1f- delay));
        sequence.Append(canvasGroup.DOFade(0,0.5f).SetDelay(1f - delay));
    }
}
