using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class TransfromTarget : Target
    {
        public override Vector3 Position => _target.position;
        private Transform _target;

        public TransfromTarget(Transform target)
        {
            _target = target;
        }
    }
}
