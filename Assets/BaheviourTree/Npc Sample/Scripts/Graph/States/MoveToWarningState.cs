using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Move to warning")]
    public class MoveToWarningState : NpcState
    {
        [SerializeField, Min(0)] private int _dangerDistance = 9;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasEnemy, _noEnemy;

        public override async Task<NodePort> Execute(Npc agent)
        {
            agent.Mover.MoveTo(agent.TargetWarning);
            agent.Mover.LookAt(agent.TargetWarning);

            while (agent.Mover.IsMove)
            {
                if (TryReactOnEnemy(agent, _dangerDistance))
                    return GetOutputPort(nameof(_hasEnemy));
                await Task.Delay(100);
            }

            return GetOutputPort(nameof(_noEnemy));
        }
    }
}
