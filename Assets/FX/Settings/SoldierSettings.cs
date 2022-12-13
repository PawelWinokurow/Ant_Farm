using UnityEngine;

[CreateAssetMenu]
public class SoldierSettings : FighterSettings
{
    public new readonly int HP = 150;
    public new readonly int ATTACK_STRENGTH = 5;
    public new readonly int PATROL_MOVEMENT_SPEED = 5;
    public new readonly int FOLLOWING_MOVEMENT_SPEED = 10;
}