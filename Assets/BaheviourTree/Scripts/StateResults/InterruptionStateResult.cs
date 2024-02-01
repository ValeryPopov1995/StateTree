namespace ValeryPopov.Common.StateTree
{
    public class InterruptionStateResult<TAgent> : StateResult<TAgent> where TAgent : Agent
    {
        private Interruption<TAgent> _interruption;

        public InterruptionStateResult(Interruption<TAgent> interruption)
        {
            _interruption = interruption;
        }

        public override State<TAgent> Complete(Agent<TAgent> agent)
        {
            agent.UnhandledInterrupt = _interruption;
            return agent.InterruptionState;
        }
    }
}
