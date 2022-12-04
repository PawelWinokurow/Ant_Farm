using System;

namespace GunnerNamespace
{
    public class GunnerTarget
    {
        public string id { get; set; }
        public Gunner gunner { get; set; }
        public Mob mob { get; set; }
        public FloorHexagon hex { get; set; }
        public Path path { get; set; }
        public Action Cancel { get; set; }

        public GunnerTarget(string id, Gunner gunner, Mob target)
        {
            this.id = id;
            this.gunner = gunner;
            this.mob = target;
            hex = target.currentHex;
        }
    }
}