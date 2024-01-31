using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class Cover : MonoBehaviour
    {
        public abstract Target GetCoverTarget(Npc npc);
    }
}
