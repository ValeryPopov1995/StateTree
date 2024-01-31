using System.Threading.Tasks;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    /// <summary>
    /// Return to StartNode
    /// </summary>
    [CreateNodeMenu("StateTree/Npc Sample/Base/Exit")]
    public class ExitState : NpcState
    {
        [Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _empty;

        public override async Task<NodePort> Execute(Npc agent)
        {
            await Task.Yield();
            return GetOutputPort(nameof(_empty));
        }
    }
}
