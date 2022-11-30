using UnityEngine;
using DG.Tweening;

[ExecuteInEditMode]

public class Trap_01 : MonoBehaviour
{
    public Transform spikes;
    public Transform body;
    public bool isPlay;
    void Start()
    {
        spikes.localScale = Vector3.zero;
        body.localEulerAngles = Vector3.up * Random.Range(0, 6) * 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!isPlay)
            {
                Play();
            }

        }


    }
    public void Play()
    {
        isPlay = true;
        Sequence s = DOTween.Sequence();
        s.Append(spikes.DOScale(new Vector3(1f, 0, 1f), 0f));
        s.Append(spikes.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).OnComplete(() => { Attack(); }));
        s.AppendInterval(0.5f);
        s.Append(spikes.DOScale(new Vector3(1f, 0, 1f), 0.1f).SetEase(Ease.Linear).OnComplete(() => { spikes.localScale = Vector3.zero; }));
        s.AppendInterval(0.5f);
        s.OnComplete(() => { isPlay = false; });
    }
    private void Attack()
    {
        Debug.Log("Attack");
    }

}


