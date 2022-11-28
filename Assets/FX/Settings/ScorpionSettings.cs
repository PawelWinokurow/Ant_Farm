using UnityEngine;

[CreateAssetMenu]
public class ScorpionSettings : ScriptableObject
{
    public readonly int HP = 250;
    public readonly int ATTACK_STRENGTH = 10;
    public readonly int FOLLOWING_MOVEMENT_SPEED = 10;
    public readonly int PATROL_MOVEMENT_SPEED = 5;
    public readonly int DIG_MOVEMENT_SPEED = 3;
}