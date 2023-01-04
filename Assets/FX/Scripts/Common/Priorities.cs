using System.Collections.Generic;

static class Priorities
{
    public static readonly Dictionary<ACTOR_TYPE, int> ENEMY_TARGET_PRIORITIES = new Dictionary<ACTOR_TYPE, int>(){
        {ACTOR_TYPE.TURRET, 1},
        {ACTOR_TYPE.SOLDIER, 1},
        {ACTOR_TYPE.GUNNER, 1},
        {ACTOR_TYPE.WORKER, 2}
};
    public static readonly Dictionary<ACTOR_TYPE, int> ALLIES_TARGET_PRIORITIES = new Dictionary<ACTOR_TYPE, int>(){
        {ACTOR_TYPE.BLOB, 1},
        {ACTOR_TYPE.GOBBER, 1},
        {ACTOR_TYPE.SCORPION, 1},
        {ACTOR_TYPE.ZOMBIE, 1},
};
}
