using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is low health")]
    public class IsHealthLowState : NpcState
    {
        [SerializeField] private byte lowHealth = 50;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _low, _sufficient;

        public override async Task<NodePort> Execute(Npc agent)
        {
            await Task.Yield();
            if (agent.Health.Value <= lowHealth)
                return GetOutputPort(nameof(_low));
            else
                return GetOutputPort(nameof(_sufficient));
        }
    }
}
