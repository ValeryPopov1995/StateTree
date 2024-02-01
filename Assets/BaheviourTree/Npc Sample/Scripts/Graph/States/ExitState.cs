using System.Threading.Tasks;

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

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_empty)));
        }
    }
}
