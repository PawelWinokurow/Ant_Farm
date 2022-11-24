using System;

namespace SoldierNamespace
{
    public class SoldierTarget
    {
        public string id { get; set; }
        public Soldier soldier { get; set; }
        public Mob mob { get; set; }
        public FloorHexagon hex { get; set; }
        public Path path { get; set; }
        public Action Cancel { get; set; }

        public SoldierTarget(string id, Soldier soldier, Mob target)
        {
            this.id = id;
            this.soldier = soldier;
            this.mob = target;
            hex = target.currentHex;
        }
    }
}