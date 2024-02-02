using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is low health")]
    public class IsHealthLowState : NpcState
    {
        [SerializeField, Range(1, 99)] private int lowHealthPercent = 50;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _low, _sufficient;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            if (agent.Health.Value01 * 100 <= lowHealthPercent)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_low)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_sufficient)));
        }
    }
}
