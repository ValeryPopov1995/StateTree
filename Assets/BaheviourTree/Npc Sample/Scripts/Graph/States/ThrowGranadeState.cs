using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Throw granade")]
    public class ThrowGranadeState : GetItemState
    {
        [SerializeField, Min(0)] private float _minDistance = 5;
        [SerializeField, Min(0)] private int _throwDuration = 1;
        [SerializeField] private bool _tryGetTargetFromOrder = true;

        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _thown, _noGranade, _notDistanced, _noTarget;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            await Task.Yield();

            var granade = await GetFromAnywhere(agent) as Granade;
            if (granade == null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noGranade)));

            await Task.Delay(TimeSpan.FromSeconds(_throwDuration));

            Vector3 targetPosition = default;
            if (_tryGetTargetFromOrder && agent.OrderSystem.LastOrder is ThrowGranadeOrder)
            {
                targetPosition = (agent.OrderSystem.LastOrder as ThrowGranadeOrder).Enemy.transform.position;
                agent.OrderSystem.LastOrder = null;
            }
            else if (agent.TargetEnemy)
            {
                if (Vector3.Distance(agent.transform.position, agent.TargetEnemy.transform.position) < _minDistance)
                    return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_notDistanced)));

                targetPosition = agent.TargetEnemy.transform.position;
            }
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noTarget)));

            Vector3 force = CalculateGranadeForce(agent.transform.position, targetPosition);
            granade.Throw(agent.GranadeThrowPoint.position, force);
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_thown)));
        }

        public static Vector3 CalculateGranadeForce(Vector3 from, Vector3 to)
        {
            const float forceMultiply = .9f;
            Vector3 force = to - from; // to enemy
            return (Vector3.up * force.magnitude + force) / 2 * forceMultiply; // diagonal
        }
    }
}
