using UnityEngine;

[CreateAssetMenu]
public class GobberSettings : ScriptableObject
{
    public readonly int HP = 100;
    public readonly int ATTACK_STRENGTH = 5;
    public readonly int PATROL_MOVEMENT_SPEED = 5;
    public readonly int FOLLOWING_MOVEMENT_SPEED = 10;
}