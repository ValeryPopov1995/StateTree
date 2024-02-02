using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Patrol")]
    public class PatrolState : NpcState
    {
        [SerializeField, Min(0)] private int _warningDistance = 9;
        [SerializeField, Min(0)] private int _dangerDistance = 7;
        [SerializeField, Min(0)] private Vector2 _wait = new(1, 2);

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasEnemy, _hasWarning, _noWarning;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            float duration = Random.Range(_wait.x, _wait.y);

            agent.Mover.LookAt(null);

            if (TryReactOnEnemy(agent, _dangerDistance))
            {
                WorldLog.Log(agent.TargetEnemy.transform.position, "danger", agent);
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasEnemy)));
            }

            var warningEnemies = agent.OverlapNpcs(_warningDistance).Where(a => a.TeamTag != agent.TeamTag);
            if (warningEnemies.Count() > 0)
            {
                agent.TargetWarning = new TransfromTarget(warningEnemies.First().transform);
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasWarning)));
            }

            agent.Mover.MoveTo(new PointTarget(agent.transform.position + Random.onUnitSphere * 5));
            await Task.Delay(TimeSpan.FromSeconds(duration));
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noWarning)));
        }
    }
}