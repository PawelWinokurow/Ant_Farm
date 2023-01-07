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
    public int accessMask { get; set; }
    public GameSettings gameSettings;
    public TrapSettings trapSettings;
    public WorkHexagon workHexagon;
    public Transform body;
    public Transform angl;
    protected Quaternion smoothRot;
    public DigJob digJob;
    public Target target;
    public bool isDead { get => health.isDead; }
    protected SurfaceOperations surfaceOperations { get; set; }
    protected Surface surface { get; set; }
    protected Store store;

    protected void InitSingletons()
    {
        surface = Surface.Instance;
        surfaceOperations = SurfaceOperations.Instance;
        store = Store.Instance;
    }
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

    public virtual void Attack()
    {

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


    public virtual void Hit(int damage)
    {
        health.Hit(damage);
        if (isDead)
        {
            Kill();
        }
    }

    public virtual bool IsTargetInSight()
    {
        return false;
    }

    public virtual void Rotation() { }



}


