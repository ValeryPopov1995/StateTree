using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;
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

        public override async Task<NodePort> Execute(Npc agent)
        {
            float duration = Random.Range(_wait.x, _wait.y);

            if (TryReactOnEnemy(agent, _dangerDistance))
                return GetOutputPort(nameof(_hasEnemy));

            var warningEnemies = agent.OverlapNpcs(_warningDistance).Where(a => a.TeamTag != agent.TeamTag);
            if (warningEnemies.Count() > 0)
            {
                agent.TargetWarning = new TransfromTarget(warningEnemies.First().transform);
                return GetOutputPort(nameof(_hasWarning));
            }

            agent.Mover.MoveTo(new PointTarget(agent.transform.position + Random.onUnitSphere * 5));
            await Task.Delay(TimeSpan.FromSeconds(duration));
            return GetOutputPort(nameof(_noWarning));
        }
    }
}