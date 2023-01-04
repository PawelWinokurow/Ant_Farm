using System;
public interface Targetable : Hitable, Hexable, StateMachine, Animatable
{
    public string id { get; set; }
    public ACTOR_TYPE type { get; set; }
}

namespace FighterNamespace
{
    public class Target
    {
        public string id { get; set; }
        public Targetable mob { get; set; }
        public FloorHexagon hex { get; set; }
        public Path path { get; set; }
        public Action Cancel { get; set; }
        public bool IsDead { get => mob.currentState.type == STATE.DEAD; }

        public Target(string id, Targetable mob)
        {
            this.id = id;
            this.mob = mob;
            hex = mob.currentHex;
        }
    }
}
