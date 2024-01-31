using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Magazine : Item
    {
        public bool IsEmpty => CurrentCapacity == 0;
        public bool IsFull => CurrentCapacity == Capacity;
        [field: SerializeField] public int Capacity { get; private set; } = 20;
        [field: SerializeField] public int CurrentCapacity { get; set; } = 20;

        // TODO CurrentCapacity { get; private set; }
        // TODO public bool TryGetBullets(int getted = 1) { }
    }
}