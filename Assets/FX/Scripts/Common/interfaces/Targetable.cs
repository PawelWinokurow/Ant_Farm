using UnityEngine;

public interface Targetable : Hitable, Hexable, StateMachine, Animatable
{
    public string id { get; set; }
    public ACTOR_TYPE type { get; set; }
    public GameObject obj { get; }
}