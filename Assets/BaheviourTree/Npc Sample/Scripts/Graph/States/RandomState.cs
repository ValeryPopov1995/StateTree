using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Random")]
    public class RandomState : NpcState
    {
        [field: SerializeField, Output(typeConstraint = TypeConstraint.Strict)]
        private NpcState _states;

        public override async Task<NodePort> Execute(Npc agent)
        {
            await Task.Yield();
            return GetOutputPort(nameof(_states));
        }

        public override NodePort GetCustomConnection()
        {
            var cncts = GetOutputPort(nameof(_states)).GetConnections();
            return cncts[Random.Range(0, cncts.Count)];
        }
    }
}