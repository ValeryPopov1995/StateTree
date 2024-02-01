using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Move to start point")]
    public class MoveToStartPointState : NpcState
    {
        [SerializeField] private bool _lookAtStart;
        [SerializeField] private int _dangerDistance = 7;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _complete, _hasEnemy;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            agent.Mover.MoveTo(new PointTarget(agent.StartPosition));
            if (_lookAtStart)
                agent.Mover.LookAt(new PointTarget(agent.StartPosition));

            while (agent.Mover.IsMove)
            {
                if (TryReactOnEnemy(agent, _dangerDistance))
                    return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasEnemy)));
                await Task.Delay(100);
            }

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_complete)));
        }
    }
}
