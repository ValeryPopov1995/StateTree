using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Tell")]
    public class TellState : NpcState
    {
        [SerializeField] private CommunicationCommandType _command;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _next;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            agent.Communication.Tell(_command);
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_next)));
        }
    }
}
