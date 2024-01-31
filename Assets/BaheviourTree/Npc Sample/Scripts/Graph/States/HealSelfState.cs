using System;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Heal Self")]
    public class HealSelfState : UseItemNpcState
    {
        [SerializeField, Min(0)] private float _healDuration = 2.5f;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _healed, _failed;

        public override async Task<NodePort> Execute(Npc agent)
        {
            var medpack = GetFromAnywhere(agent);
            if (medpack == null)
                return GetOutputPort(nameof(_failed));

            await Task.Delay(TimeSpan.FromSeconds(_healDuration));
            (medpack as Medpack).Heal(agent);
            return GetOutputPort(nameof(_healed));
        }
    }
}
