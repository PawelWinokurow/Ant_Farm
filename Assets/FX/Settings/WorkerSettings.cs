using UnityEngine;

[CreateAssetMenu]
public class WorkerSettings : ScriptableObject
{
    public int HP = 100;
    public float CONSTRUCTION_SPEED = 10f;
    public int LOADING_SPEED = 50;
    public int UPLOADING_SPEED = 400;
    public int MAX_CARRYING_WEIGHT = 100;
    public int MOVEMENT_SPEED = 5;
    public int ACCESS_MASK = 5;

}