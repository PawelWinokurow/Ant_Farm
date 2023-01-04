using System;
using UnityEngine;
using TrapNamespace;
using FighterNamespace;

public class Trap : MonoBehaviour, Targetable
{
    public string id { get; set; }
    public ACTOR_TYPE type { get; set; }
    public IMobAnimator animator { get; set; }
    public Health health { get; set; }
    public Vector3 position { get => transform.position; }
    public State currentState { get; set; }
    public FloorHexagon currentHex { get; set; }
    public Action Kill { get; set; }
    public Store store;
    public int accessMask { get; set; }
    public GameSettings gameSettings;
    public TrapSettings trapSettings;
    public WorkHexagon workHexagon;
    public Transform body;
    public Transform angl;
    protected Quaternion smoothRot;
    public DigJob digJob;
    public Target target;


    public void SetInitialState()
    {
        SetState(new IdleState(this));
    }

    public void SetTarget(Target target)
    {
        this.target = target;
    }

    public virtual Target SearchTarget()
    {
        return null;
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;
        gameObject.name = type.ToString() + " - " + state.GetType().Name;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    void Update()
    {
        currentState.Tick();
    }


    public void SetRunAnimation()
    {
        animator.Run();
    }
    public void SetIdleAnimation()
    {
        animator.Idle();
    }
    public void SetIdleFightAnimation()
    {
        animator.IdleFight();
    }


    public virtual float Hit(int damage)
    {
        if (health.hp > 0 && health.Hit(damage) <= 0)
        {
            Kill();
        }
        return health.hp;
    }

    public virtual bool IsTargetInSight()
    {
        return false;
    }

    public virtual void Rotation() { }



}


