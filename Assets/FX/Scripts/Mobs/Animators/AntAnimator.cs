using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimator : MonoBehaviour
{

    public Worker worker;
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

    public GameObject deadFX_prefab;
    public HealthPopUp popUp_prefad;

    MaterialPropertyBlock props;
    private float a;

    private void Start()
    {
        posOld = transform.position;
        antBaseForwardOld = transform.forward;
        current = run;
        props = new MaterialPropertyBlock();
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
        if (worker != null)
        {

            //t += Time.deltaTime / (1f / 30f) * current.speed;
            f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
            mf.mesh = current.sequence[f];

            if (worker.currentPathEdge != null)
            {
                forward = worker.currentPathEdge.to.position - transform.position;
            }
            else if (worker.job?.destination != null)
            {
                forward = worker.job.destination - transform.position;
            }

            Quaternion rot = Quaternion.LookRotation(angl.up, forward);
            smoothRot = Quaternion.Slerp(smoothRot, rot, Time.deltaTime * 10f);
            mf.transform.rotation = smoothRot;
            mf.transform.Rotate(new Vector3(-90f, 0f, 180f), Space.Self);
        }


        if (a >= 0)
        {
            props.SetFloat("_Blink_FX", a);
            mr.SetPropertyBlock(props);
            a -= Time.deltaTime / 0.1f;
        }
    }

    public void Dead()
    {
        StartCoroutine(Dead_Cor());
    }
    private IEnumerator Dead_Cor()
    {
        
        GameObject fx = Instantiate(deadFX_prefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.15f);
        mr.enabled = false;
    }
    public void MakeVisible()
    {
        mr.enabled = true;
    }
    public void Hit(int damage)
    {
        HealthPopUp healthPopUp = Instantiate(popUp_prefad, transform.position, transform.rotation);
        healthPopUp.tmp.color = healthPopUp.damageColor;
        a = 1f;
    }
}
