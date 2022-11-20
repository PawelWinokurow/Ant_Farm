using SoldierNamespace;
using UnityEngine;


public class SoldierAnimator : MonoBehaviour
{
    public Soldier soldier;
    private AnimationsScriptableObject current;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject run;

    private int f;
    public MeshFilter mf;
    public MeshRenderer mr;
    private Vector3 forward;
    private Quaternion smoothRot;
    public Transform angl;

    public float startTime;
    public bool isHitMade = false;
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

    public void Idle()
    {
        current = idle;
        mr.materials = current.materials;
    }


    void Update()
    {
        if (soldier != null)
        {
            f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
            mf.mesh = current.sequence[f];
            if (soldier.currentState.type == STATE.ATTACK && soldier.target?.mob != null)
            {
                forward = soldier.target.mob.position - transform.position;
            }
            else if (soldier.currentPathEdge != null)
            {
                forward = soldier.currentPathEdge.to.position - transform.position;
            }
            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            mf.transform.rotation = smoothRot;
            mf.transform.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
            if (soldier.currentState.type == STATE.ATTACK)
            {
                var fFloat = (Time.time - startTime) * 30f * 1.5f;
                f = (int)(fFloat) % current.sequence.Length;
                var rest = fFloat - (int)fFloat;

                if (fOld > (f + rest))
                {
                    isHitMade = false;
                }

                if (!isHitMade && (f + rest) >= 14)
                {
                    soldier.Attack();
                    isHitMade = true;
                }
                fOld = f + rest;
            }
        }
    }
    public void ResetAttack()
    {
        startTime = Time.time;
        isHitMade = false;
        fOld = 0f;
    }

}

