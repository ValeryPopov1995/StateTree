namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class CoverOrder : Order
    {
        public CoverOrder(Target target)
        {
            Target = target;
        }

        public Target Target { get; private set; }
    }
}