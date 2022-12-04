using UnityEngine;
using EnemyNamespace;
using UnityEngine.Events;

public class Animator_Mobs : MonoBehaviour
{
    public Enemy enemy;
    private AnimationsScriptableObject current;
    public AnimationsScriptableObject run;
    public AnimationsScriptableObject idleFight;
    public AnimationsScriptableObject runFood;

    public int f = 0;
    public MeshFilter mf;
    public MeshRenderer mr;
    private Vector3 forward;
    private Quaternion smoothRot;
    public Transform angl;

    public bool isHitMade ;
    private float fOld;

   public  UnityEvent m_Shoot;

    private void Start()
    {
        current = run;

    }

    public void Run()
    {
        current = run;
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

        if (enemy != null)
        {
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
                current = idleFight;
            }
        }

        f = (int)(Time.time * 30f * 1.5f) % current.sequence.Length;
        mf.mesh = current.sequence[f];

        if (current == idleFight)
        {
            if (f > 14 && !isHitMade)
            {
                isHitMade = true;
                // enemy.Attack();//?
                m_Shoot.Invoke();
            }

            if (fOld < 14 && isHitMade)
            {
                isHitMade = false;
            }
        }
        fOld = f;
    }

}

