using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is Has Order")]
    public class IsHasOrderState : NpcState
    {
        [SerializeField, Order] private string _order;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasThisOrder, _noOrder;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            if (agent.OrderSystem.LastOrder != null && agent.OrderSystem.LastOrder.GetType().AssemblyQualifiedName == _order)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasThisOrder)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noOrder)));
        }
    }
}
