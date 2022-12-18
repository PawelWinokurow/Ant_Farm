using UnityEngine;
using UnityEngine.Events;
using System;

public class MobAnimator : MonoBehaviour, IMobAnimator
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
    private int fMod;
    private float fModOld;

    public UnityEvent m_Shoot;

    //public Action Attack;

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
        f = (int)(Time.time * 30f * 1.5f);
        fMod = f % current.sequence.Length;
        mf.mesh = current.sequence[fMod];


            if (fMod > 14 && !isHitMade)
            {
                isHitMade = true;
                if (current == idleFight)
                {
                    m_Shoot.Invoke();
                }
            }

            if (fModOld < 14 && isHitMade)
            {
                isHitMade = false;
            }

         fModOld = fMod;

    }

    }


