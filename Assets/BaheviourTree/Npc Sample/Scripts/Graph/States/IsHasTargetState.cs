using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is has Target")]
    public class IsHasTargetState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasTarget, _noTarget;

        public override async Task<NodePort> Execute(Npc agent)
        {
            await Task.Yield();
            if (agent.TargetEnemy == null)
                return GetOutputPort(nameof(_noTarget));
            else
                return GetOutputPort(nameof(_hasTarget));
        }
    }
}
