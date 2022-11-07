using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimator : MonoBehaviour
{
    private AnimationsScriptableObject current;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idle;
    public AnimationsScriptableObject runFood;

    private int f;
    public MeshFilter mf;
    public MeshRenderer mr;
    private float t;

    private Vector3 forward;
    private Vector3 antBaseForwardOld;
    private Vector3 posOld;
    private Quaternion smoothRot;
    public Transform angl;

    private void Start()
    {
        posOld = transform.position;
        antBaseForwardOld = transform.forward;
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
    public void RunFood()
    {
        current = runFood;
        mr.materials = current.materials;
    }
    void Update()
    {
        //t += Time.deltaTime / (1f / 30f) * current.speed;
        f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
        mf.mesh = current.sequence[f];


        Vector3 pos = transform.position;
        if (pos != posOld)
        {
            forward = ((pos - posOld).normalized + antBaseForwardOld * 0.001f).normalized;
            antBaseForwardOld = mf.transform.forward;
            posOld = pos;
        }


        Quaternion rot = Quaternion.LookRotation(forward, angl.up);
        smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
        mf.transform.rotation = smoothRot;

    }


}