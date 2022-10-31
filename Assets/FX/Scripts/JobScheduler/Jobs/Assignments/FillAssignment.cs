using System.Collections;
using UnityEngine;


public class FillAssignment : Assignment
{
    private Hexagon hex;
    private Mob mob;

    public AssignmentType Type { get; set; }
    public FillAssignment(Hexagon hex)
    {
        this.hex = hex;
        Type = AssignmentType.FILL;
    }
    public void Execute(Mob mob)
    {
        this.mob = mob;
        Debug.Log("Fill");
    }

}