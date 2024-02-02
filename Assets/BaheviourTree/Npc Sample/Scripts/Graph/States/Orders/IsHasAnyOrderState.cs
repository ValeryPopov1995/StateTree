using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is Has Any Order")]
    public class IsHasAnyOrderState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _noOrder;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            // see NpcState._hasOrder
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noOrder)));
        }
    }
}
