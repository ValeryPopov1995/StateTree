using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Toggle")]
    public class ToggleState : NpcState
    {
        [SerializeField] private bool _toggle;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _true, _false;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            if (_toggle)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_true)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_false)));
        }
    }
}
