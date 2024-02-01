using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Medpack : Item
    {
        [SerializeField, Min(1)] private int _healPoints = 10;

        public void Heal(Npc npc)
        {
            npc.Health.GetHeal(_healPoints);
            DropFromInventory();
            Destroy(gameObject);
        }
    }
}
