using UnityEngine;
using UnityEngine.Events;

public class MobAnimatorBlob : MonoBehaviour, IMobAnimator
{
    public int f = 0;
    public bool isHitMade;
    private float fOld;
    public UnityEvent m_Shoot;
    private enum StateType
    {
        IdleFight, Run, Idle
    }
    private StateType state;

    public ParticleSystem tentackles;
    MaterialPropertyBlock props;
    public MeshRenderer mr;
    private float t;
    private float a;

    private void Start()
    {
        props = new MaterialPropertyBlock();
        Run();
    }

    public void Run()
    {
        t = 0;
        a = 0;
        state = StateType.Run;
        tentackles.Stop();
    }
    public void Idle()
    {
        state = StateType.Idle;
        tentackles.Stop();
    }
    public void IdleFight()
    {
        state = StateType.IdleFight;
    }
    public void RunFood()
    {
    }

    void Update()
    {
        f = (int)(Time.time * 30f * 1.5f) % 32;

        if (f > 14 && !isHitMade)
        {
            isHitMade = true;
            if (state == StateType.IdleFight)
            {
                m_Shoot.Invoke();
                tentackles.Play();
                mr.transform.localScale = 1.5f * Vector3.one;
            }
        }

        if (fOld < 14 && isHitMade)
        {
            isHitMade = false;
            tentackles.Stop();
        }

        fOld = f;

        if (mr.transform.localScale != Vector3.one)
        {
            mr.transform.localScale = Vector3.Lerp(mr.transform.localScale, Vector3.one, Time.deltaTime * 20f);
        }

        if (state == StateType.Run)
        {
            t += Time.deltaTime * 5f;
            a = ExtensionMethods.Remap(Mathf.Sin(t - Mathf.PI / 2f), -1f, 1f, 0f, 1f);
            props.SetFloat("_Forward", a);
            mr.SetPropertyBlock(props);
        }
        else
        {
            if (a > 0)
            {
                a = Mathf.Lerp(a, 0, Time.deltaTime * 10f);
                a = Mathf.Max(a, 0);
                props.SetFloat("_Forward", a);
                mr.SetPropertyBlock(props);

            }
        }
    }

}

