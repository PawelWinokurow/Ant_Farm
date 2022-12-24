using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MobAnimatorTurret : MonoBehaviour, IMobAnimator
{
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject idleFight;
    private AnimationsScriptableObject current;
    public MeshFilter mf;
    public Transform tower;
    private Tween tween;
    public int f = 0;
    private int fMod;
    private float fModOld;
    public bool isHitMade;
    public Transform cannonBody;
    public UnityEvent m_Shoot;

    void Start()
    {
        Idle();
    }

    public void Run()
    {
    }
    public void Idle()
    {
        if (tween != null)
        {
            tween.Kill();
        }

        current = idle;
        float angl = Random.Range(-90f, 90f);
        tween = tower.transform.DOLocalRotate(Vector3.up * angl, Mathf.Abs(angl) * 0.01f, RotateMode.LocalAxisAdd)
              .SetEase(Ease.InOutQuad)
              .SetDelay(Random.Range(1f, 3f))
              .OnStart(() => { current = run; })
              .OnComplete(() => { Idle(); });
    }

    public void IdleFight()
    {
        current = idleFight;
        tween.Kill();
        tween = tower.DOLocalRotate(Vector3.zero, 0.3f, RotateMode.Fast).SetEase(Ease.OutQuad).OnStart(() => { });
        /*
            tween.Kill();
            Quaternion look = Quaternion.LookRotation(tower.parent.up, enemyPos - transform.position);
            look *= Quaternion.Euler(new Vector3(-90, 0, 180));
            tween = tower.DORotateQuaternion(look, 0.3f).SetEase(Ease.OutQuad).OnStart(() => {
    });
 */

    }

    public void RunFood()
    {
    }
    private void Update()
    {
        f = (int)(Time.time * 30f * 1.5f);
        fMod = f % current.sequence.Length;
        mf.mesh = current.sequence[fMod];


        if (fMod > 14 && !isHitMade)
        {
            isHitMade = true;
            if (current == idleFight)
            {
                cannonBody.localPosition = -0.5f * Vector3.forward;
                cannonBody.DOLocalMoveZ(0, 0.2f).SetDelay(0.1f);
                m_Shoot.Invoke();
            }
        }

        if (fModOld < 14 && isHitMade)
        {
            isHitMade = false;
        }

        fModOld = fMod;
    }
    public void Shoot()
    {
        Debug.Log("Shoot");
    }

}

