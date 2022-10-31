using System;

public class AssignmentFactory
{
    public static Assignment CreateDigAssignment(Hexagon hex)
    {
        return new DigAssignment(hex);
    }
    public static Assignment CreateFillAssignment(Hexagon hex)
    {
        return new FillAssignment(hex);

    }
}
