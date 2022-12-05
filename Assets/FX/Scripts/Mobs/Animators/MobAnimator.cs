using UnityEngine;
using UnityEngine.Events;
using System;

public class MobAnimator : MonoBehaviour
{
    public AnimationsScriptableObject current;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject idleFight;
    public AnimationsScriptableObject runFood;

    public int f = 0;
    public MeshFilter mf;
    public MeshRenderer mr;
    public bool isHitMade;
    private float fOld;

    public UnityEvent m_Shoot;

    public Action Attack;

    private void Start()
    {
        current = run;
    }

    public void Run()
    {
        current = run;
        mr.materials = current.materials;
    }
    public void Idle()
    {
        current = idle;
        mr.materials = current.materials;
    }
    public void IdleFight()
    {
        current = idleFight;
        mr.materials = current.materials;
    }
    public void RunFood()
    {
        current = runFood;
        mr.materials = current.materials;
    }


    void Update()
    {
        f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
        mf.mesh = current.sequence[f];

        if (current == idleFight)
        {
            if (f > 14 && !isHitMade)
            {
                isHitMade = true;
                 m_Shoot.Invoke();
            }

            if (fOld < 14 && isHitMade)
            {
                isHitMade = false;
            }
        }
        fOld = f;
    }

    public void ResetAttack()
    {
        isHitMade = false;
        fOld = 0f;
    }

}

