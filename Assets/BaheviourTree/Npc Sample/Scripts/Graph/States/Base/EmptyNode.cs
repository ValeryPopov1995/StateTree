using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Node")]
    public class EmptyNode : NpcState
    {
        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _next;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_next)));
        }
    }
}
