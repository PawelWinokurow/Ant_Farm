using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimator : MonoBehaviour
{
    private AnimationsScriptableObject current;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idle;

    private int f;
    public MeshFilter mf;
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
    }
    public void Idle()
    {
        current = idle;
    }
    void Update()
    {
        t += Time.deltaTime / (1f / 30f) * current.speed;
        f = (int)t % current.sequence.Length;
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