using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Cover")]
    public class CoverState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _covered, _noCover;

        public override async Task<NodePort> Execute(Npc agent)
        {
            float time = Time.time;
            var cover = agent.FindCover();
            if (cover)
            {
                agent.Mover.MoveTo(cover.GetCoverTarget(agent));
                bool maxDuration() => time > Time.time + 3;
                await UniTask.WaitWhile(() => agent.Mover.IsMove || maxDuration());

                if (maxDuration())
                    return GetOutputPort(nameof(_noCover));

                return GetOutputPort(nameof(_covered));
            }

            return GetOutputPort(nameof(_noCover));
        }
    }
}