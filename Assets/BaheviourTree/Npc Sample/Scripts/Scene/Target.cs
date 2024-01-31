using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class Target
    {
        public abstract Vector3 Position { get; }

        public static implicit operator Vector3(Target target) => target.Position;
    }
}
