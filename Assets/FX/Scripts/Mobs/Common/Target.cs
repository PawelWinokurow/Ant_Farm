using System;


public class Target
{
    public string id { get; set; }
    public Mob mob { get; set; }
    public Mob target { get; set; }
    public FloorHexagon hex { get; set; }
    public Path path { get; set; }
    public Action Cancel { get; set; }

    public Target(string id, Mob mob, Mob target)
    {
        this.id = id;
        this.mob = mob;
        this.target = target;
        hex = target.currentHex;
    }
}
