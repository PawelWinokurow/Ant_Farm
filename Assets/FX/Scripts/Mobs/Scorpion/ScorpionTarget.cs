using System;

namespace ScorpionNamespace
{
    public class ScorpionTarget
    {
        public string id { get; set; }
        public Scorpion scorpion { get; set; }
        public Mob mob { get; set; }
        public FloorHexagon hex { get; set; }
        public Path path { get; set; }
        public Action Cancel { get; set; }

        public ScorpionTarget(string id, Scorpion scorpion, Mob target)
        {
            this.id = id;
            this.scorpion = scorpion;
            this.mob = target;
            hex = target.currentHex;
        }
    }
}