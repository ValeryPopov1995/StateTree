using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class EmptyTarget : Target
    {
        public override Vector3 Position => default;

        public override bool IsEmpty => true;
    }
}
