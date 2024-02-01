namespace ValeryPopov.Common.StateTree
{
    public abstract class StateResult<TAgent> where TAgent : Agent
    {
        public abstract State<TAgent> Complete(Agent<TAgent> agent);
    }
}
