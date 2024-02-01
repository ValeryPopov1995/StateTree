using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Random")]
    public class RandomState : NpcState
    {
        [field: SerializeField, Output(typeConstraint = TypeConstraint.Strict)]
        private NpcState _states;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            var connections = GetOutputPort(nameof(_states)).GetConnections();
            var nextStatePort = connections[Random.Range(0, connections.Count)];
            return new InputPortStateResult<Npc>(nextStatePort);
        }
    }
}