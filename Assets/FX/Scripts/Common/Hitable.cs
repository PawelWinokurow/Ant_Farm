using System;

public interface Hitable
{
    public void Hit(int damage);
    public bool isDead { get; }
    public Health health { get; set; }
    public Action Kill { get; set; }
}