using UnityEngine;

[CreateAssetMenu]
public class WorkerSettings : ScriptableObject
{
    public int HP = 100;
    public int CONSTRUCTION_SPEED = 10;
    public int LOADING_SPEED = 5;
    public int MAX_CARRYING_WEIGHT = 10;
    public int MOVEMENT_SPEED = 5;
    public int ACCESS_MASK = 5;

}