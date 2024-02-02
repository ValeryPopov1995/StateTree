using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Random")]
    public class RandomState : NpcState
    {
        [SerializeField, Range(0, 100)] private int _statesChance = 50;

        [SerializeField, Output(typeConstraint = TypeConstraint.Strict)]
        private NpcState _states, _exit;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();

            if (Random.Range(0, 100) > _statesChance)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_exit)));

            var connections = GetOutputPort(nameof(_states)).GetConnections();
            var nextStatePort = connections[Random.Range(0, connections.Count)];
            return new InputPortStateResult<Npc>(nextStatePort);
        }
    }
}