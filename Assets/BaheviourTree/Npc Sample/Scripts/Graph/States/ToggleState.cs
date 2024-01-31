using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Toggle")]
    public class ToggleState : NpcState
    {
        [SerializeField] private bool _toggle;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _true, _false;

        public override async Task<NodePort> Execute(Npc agent)
        {
            await Task.Yield();
            if (_toggle)
                return GetOutputPort(nameof(_true));
            else
                return GetOutputPort(nameof(_false));
        }
    }
}
