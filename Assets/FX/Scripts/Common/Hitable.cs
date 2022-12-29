using System;

public interface Hitable
{
    public float Hit(int damage);
    public Health health { get; set; }
    public Action Kill { get; set; }
}