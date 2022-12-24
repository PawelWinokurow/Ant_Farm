using MobNamespace;

namespace FighterNamespace
{
    public class DigState : State
    {
        private Scorpion scorpion;
        private DigJob job;

        public DigState(Scorpion scorpion) : base(scorpion)
        {
            this.type = STATE.DIG;
            this.scorpion = scorpion;
        }

        public override void Tick()
        {
            scorpion.surfaceOperations.Dig(job);
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