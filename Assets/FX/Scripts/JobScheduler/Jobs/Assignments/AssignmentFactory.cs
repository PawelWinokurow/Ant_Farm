using System;

public class AssignmentFactory
{
    public static Action<Mob> CreateDigAssignment(Hexagon hex)
    {
        return new Action<Mob>((Mob mob) => new DigAssignment().DoWork(hex, mob));
    }
    public static Action<Mob> CreateBuryAssignment(Hexagon hex)
    {
        return new Action<Mob>((Mob mob) => new BuryAssignment().DoWork(hex, mob));

    }
}
