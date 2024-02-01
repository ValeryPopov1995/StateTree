using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Throw granade")]
    public class ThrowGranadeState : UseItemNpcState
    {
        [SerializeField, Min(0)] private float _force = 10;
        [SerializeField, Min(0)] private float _minDistance = 5;
        [SerializeField, Min(0)] private int _throwDuration = 1;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _thown, _noGranade, _notDistanced;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();

            var granade = await GetFromAnywhere(agent) as Granade;
            if (granade == null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noGranade)));

            if (Vector3.Distance(agent.transform.position, agent.TargetEnemy.transform.position) < _minDistance)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_notDistanced)));

            await Task.Delay(TimeSpan.FromSeconds(_throwDuration));

            Vector3 force = agent.TargetEnemy.transform.position - agent.transform.position; // to enemy
            force = (Vector3.up * force.magnitude + force) / 2 * _force; // diagonal

            granade.Throw(agent.GranadeThrowPoint.position, force);
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_thown)));
        }
    }
}
