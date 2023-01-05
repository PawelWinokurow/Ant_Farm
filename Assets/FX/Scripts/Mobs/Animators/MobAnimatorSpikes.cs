using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MobAnimatorSpikes : MonoBehaviour, IMobAnimator
{
    public Health health;
    public Transform spikes;
    public Transform body;
    public UnityEvent m_Shoot;
    private Sequence s;


    private void Start()
    {
        Idle();
    }

    public void Run()
    {
    }
    public void Idle()
    {
        spikes.localScale = Vector3.zero;
    }
    public void IdleFight()
    {
        m_Shoot.Invoke();

        if (s != null)
        {
            s.Complete();
        }

        s = DOTween.Sequence();
        s.Append(spikes.DOScale(new Vector3(1f, 1.5f, 1f), 0f));
        s.Append(spikes.DOScale(new Vector3(1f, 1f, 1f), 0.1f).SetEase(Ease.OutBack).OnComplete(() => { }));
        s.AppendInterval(0.5f);
        s.Append(spikes.DOScale(new Vector3(1f, 0f, 1f), 0.1f).SetEase(Ease.Linear).OnComplete(() => { spikes.localScale = Vector3.zero; }));
        s.AppendInterval(0.5f);
        s.OnComplete(() => { Idle(); });

    }
    public void RunFood()
    {
    }
}

