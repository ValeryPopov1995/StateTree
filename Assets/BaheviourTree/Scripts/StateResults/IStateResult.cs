namespace ValeryPopov.Common.StateTree
{
    public interface IStateResult<TAgent> where TAgent : Agent
    {
        State<TAgent> Complete(Agent<TAgent> agent);
    }
}
