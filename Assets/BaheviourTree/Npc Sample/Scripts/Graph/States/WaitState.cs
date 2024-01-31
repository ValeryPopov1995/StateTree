using System;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Wait")]
    public class WaitState : NpcState
    {
        [SerializeField, Min(0)] private Vector2 _wait = new(1, 2);
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _next;

        public override async Task<NodePort> Execute(Npc agent)
        {
            float duration = UnityEngine.Random.Range(_wait.x, _wait.y);
            await Task.Delay(TimeSpan.FromSeconds(duration), agent.destroyCancellationToken);
            return GetOutputPort(nameof(_next));
        }
    }
}