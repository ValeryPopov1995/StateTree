using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Heal Self")]
    public class HealSelfState : UseItemNpcState
    {
        [SerializeField, Min(0)] private float _healDuration = 2.5f;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _healed, _failed;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            var medpack = await GetFromAnywhere(agent) as Medpack; // TODO move to item
            if (medpack == null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_failed)));

            await Task.Delay(TimeSpan.FromSeconds(_healDuration));
            medpack.Heal(agent);
            agent.Communication.Tell(CommunicationCommandType.ImHealthy);
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_healed)));
        }
    }
}
