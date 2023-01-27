namespace TrapNamespace
{
    public class Egg : Trap
    {

        void Start()
        {
            animator = GetComponent<MobAnimatorNull>();
            gameSettings = Settings.Instance.gameSettings;
            trapSettings = Settings.Instance.eggSettings;
            type = ACTOR_TYPE.EGG;
            health = GetComponent<Health>();
            workHexagon = GetComponent<WorkHexagon>();
            currentHex = workHexagon.floorHexagon;
            health.InitHp(trapSettings.HP);
            Kill = () =>
            {
                SetState(new DeadState(this));
                surface.RemoveBuilding(workHexagon.floorHexagon);
            };
            InitSingletons();
            SetInitialState();
        }
    }


}