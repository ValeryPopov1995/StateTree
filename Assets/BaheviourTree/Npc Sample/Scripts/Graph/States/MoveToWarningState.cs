using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Move to warning")]
    public class MoveToWarningState : NpcState
    {
        [SerializeField, Min(0)] private int _dangerDistance = 9;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _noWarning, _hasEnemy, _noEnemy;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            if (agent.TargetWarning.IsEmpty)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noWarning)));

            agent.Mover.MoveTo(agent.TargetWarning);
            agent.Mover.LookAt(agent.TargetWarning);

            while (agent.Mover.IsMove)
            {
                if (TryReactOnEnemy(agent, _dangerDistance))
                    return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasEnemy)));
                await Task.Delay(100);
            }

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noEnemy)));
        }
    }
}
