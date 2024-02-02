namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class ThrowGranadeOrder : Order
    {
        public ThrowGranadeOrder(Npc enemy)
        {
            Enemy = enemy;
        }

        public Npc Enemy { get; private set; }
    }
}