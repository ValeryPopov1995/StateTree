using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Heal Self")]
    public class HealSelfState : GetItemState
    {
        [SerializeField, Min(0)] private float _healDuration = 2.5f;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _healed, _failed;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            var medpack = await GetFromAnywhere(agent) as Medpack;
            if (medpack == null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_failed)));

            await Task.Delay(TimeSpan.FromSeconds(_healDuration));
            medpack.Heal(agent);
            agent.Communication.TellCommand(CommunicationCommandType.ImHealthy);
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_healed)));
        }
    }
}
