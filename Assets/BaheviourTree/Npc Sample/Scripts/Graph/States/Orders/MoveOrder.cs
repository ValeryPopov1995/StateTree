namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class MoveOrder : Order
    {
        public MoveOrder(Target target)
        {
            Target = target;
        }

        public Target Target { get; private set; }
    }
}