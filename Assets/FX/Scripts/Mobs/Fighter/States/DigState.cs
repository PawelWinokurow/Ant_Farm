namespace FighterNamespace
{
    public class DigState : State
    {
        private Scorpion scorpion;
        private DigJob job;
        private SurfaceOperations surfaceOperations;

        public DigState(Scorpion scorpion)
        {
            type = STATE.DIG;
            this.scorpion = scorpion;
            surfaceOperations = SurfaceOperations.Instance;
        }

        public override void Tick()
        {
            surfaceOperations.Dig(job);
        }

        override public void CancelJob()
        {
            job.Cancel();
        }

        override public void OnStateEnter()
        {
            job = scorpion.digJob;
            scorpion.SetIdleAnimation();
        }

        override public void OnStateExit()
        {

        }
    }
}