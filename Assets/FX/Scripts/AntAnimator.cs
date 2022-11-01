using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimator : MonoBehaviour
{
    public AnimationsScriptableObject run;

    public int f;
    public MeshFilter mf;
    private float t;

    public bool isPlay=true;

    private Vector3 forward;
    private Vector3 antBaseForwardOld;
    private Vector3 posOld;
    private Quaternion smoothRot;

    private void Start()
    {
        posOld = transform.position;
        antBaseForwardOld = transform.forward;
    }

    void Update()
    {

        if(isPlay)
        {
            //f = (int)((((distance + numInRaw * step / 3f) % step) / step) * (run.sequence.Length - 1));
            t += Time.deltaTime/(1f/30f);
            f = (int)t% run.sequence.Length;
            Debug.Log(f);
           // f = Mathf.Clamp(f, 0, run.sequence.Length);
            mf.mesh = run.sequence[f];
        }

        Vector3 pos = transform.position;
        if (pos != posOld)
        {
            /*
            if (!isRun)
            {
                isRun = true;
                anim.SetTrigger("Run");
            }
            */
        }
        else
        {
            /*
            if (isRun)
            {
               // isRun = false;
               // anim.SetTrigger("Idle");
            }
            */
        }
        forward = ((pos - posOld).normalized + antBaseForwardOld).normalized;
        Quaternion rot = Quaternion.LookRotation(forward, Vector3.up);
        smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
        mf.transform.rotation = smoothRot;
        antBaseForwardOld = mf.transform.forward;
        posOld = pos;
    }


}