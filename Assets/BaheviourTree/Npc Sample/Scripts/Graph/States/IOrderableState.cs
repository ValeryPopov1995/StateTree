namespace ValeryPopov.Common.StateTree.NpcSample
{
    public interface IOrderableState<T> where T : Order
    {
        bool TryGetTargetFromOrder { get; }
        T GetOrder(Npc agent);
    }
}
