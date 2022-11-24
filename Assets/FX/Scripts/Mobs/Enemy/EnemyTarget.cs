using System;

namespace EnemyNamespace
{
    public class EnemyTarget
    {
        public string id { get; set; }
        public Enemy enemy { get; set; }
        public Mob mob { get; set; }
        public FloorHexagon hex { get; set; }
        public Path path { get; set; }
        public Action Cancel { get; set; }

        public EnemyTarget(string id, Enemy enemy, Mob target)
        {
            this.id = id;
            this.enemy = enemy;
            this.mob = target;
            hex = target.currentHex;
        }
    }
}