using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimator : MonoBehaviour
{
    public Enemy enemy;
    private AnimationsScriptableObject current;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject idleFight;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject runFight;

    public int f = 0;
    public MeshFilter mf;
    public MeshRenderer mr;
    private Vector3 forward;
    private Quaternion smoothRot;
    public Transform angl;

    public bool isHitMade = true;
    private float fOld = 0f;

    private void Start()
    {
        current = run;
    }

    public void Run()
    {
        current = run;
        mr.materials = current.materials;
    }
    public void RunFight()
    {
        current = runFight;
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

    void Update()
    {
        if (enemy != null)
        {
            var fFLoat = Time.time * 30f * 1.5f;
            f = (int)(fFLoat) % current.sequence.Length;
            mf.mesh = current.sequence[f];
            if (enemy.currentState.type == STATE.ATTACK && enemy.target?.mob != null)
            {
                forward = enemy.target.mob.position - transform.position;
            }
            else if (enemy.currentPathEdge != null)
            {
                forward = enemy.currentPathEdge.to.position - transform.position;
            }
            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            mf.transform.rotation = smoothRot;
            mf.transform.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
            if (enemy.currentState.type == STATE.ATTACK)
            {
                var rest = fFLoat - (int)fFLoat;

                if (fOld > (f + rest))
                {
                    isHitMade = false;
                }

                if (!isHitMade && f >= 14)
                {
                    enemy.Attack();
                    isHitMade = true;
                }
                fOld = f + rest;
            }
        }
    }
}

