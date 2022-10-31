using System.Collections;


public enum AssignmentType
{
    DIG, FILL
}

public interface Assignment
{
    public AssignmentType Type { get; set; }
    public void Execute(Mob mob);
}