using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Is has Target")]
    public class IsHasTargetEnemyState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasTarget, _noTarget;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();
            if (agent.TargetEnemy == null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noTarget)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasTarget)));
        }
    }
}
