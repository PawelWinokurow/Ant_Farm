using System.Collections;
using UnityEngine;

public class DigAssignment : Assignment
{
    private Hexagon hex;
    private Mob mob;
    public AssignmentType Type { get; set; }

    public DigAssignment(Hexagon hex)
    {
        this.hex = hex;
        Type = AssignmentType.FILL;
    }

    public void Execute(Mob mob)
    {
        this.mob = mob;
        Debug.Log("Dig");
        mob.Job.RemoveJob();
    }

}