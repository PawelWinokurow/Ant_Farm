using UnityEngine;

[CreateAssetMenu]
public class WorkerSettings : ScriptableObject
{
    public readonly int HP = 100;
    public readonly float CONSTRUCTION_SPEED = 2f;
    public readonly int LOADING_SPEED = 50;
    public readonly int MAX_CARRYING_WEIGHT = 100;
    public readonly int MOVEMENT_SPEED = 5;
}