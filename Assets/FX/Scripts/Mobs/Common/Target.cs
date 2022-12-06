using System;


public class Target
{
    public string id { get; set; }
    public Mob mob { get; set; }
    public FloorHexagon hex { get; set; }
    public Path path { get; set; }
    public Action Cancel { get; set; }
    public bool IsDead { get => mob.currentState.type == STATE.DEAD; }

    public Target(string id, Mob mob)
    {
        this.id = id;
        this.mob = mob;
        hex = mob.currentHex;
    }
}
