using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class PointTarget : Target
    {
        public override Vector3 Position => _position;

        public override bool IsEmpty => _position == default;

        private Vector3 _position;

        public PointTarget(Vector3 position)
        {
            _position = position;
        }
    }
}
