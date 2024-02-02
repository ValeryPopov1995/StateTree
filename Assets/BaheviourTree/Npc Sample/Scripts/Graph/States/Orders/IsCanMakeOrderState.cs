using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is can make orders")]
    public class IsCanMakeOrderState : NpcState
    {
        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _can, _not;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            if (agent.OrderSystem.CanOrder)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_can)));

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_not)));
        }
    }
}