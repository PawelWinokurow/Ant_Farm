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

    private int f;
    public MeshFilter mf;
    public MeshRenderer mr;
    private Vector3 forward;
    private Quaternion smoothRot;
    public Transform angl;


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
            f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
            mf.mesh = current.sequence[f];

            if (enemy.CurrentPathEdge != null)
            {
                forward = enemy.CurrentPathEdge.To.Position - transform.position;
            }
            else if (enemy.Job?.Destination != null)
            {
                forward = enemy.Job.Destination - transform.position;
            }

            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            mf.transform.rotation = smoothRot;
            mf.transform.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
        }
    }

}

