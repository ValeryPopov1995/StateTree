using System.Threading.Tasks;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Start/Start")]
    public class NpcStartState : StartState<Npc>
    {
        public override async Task<IStateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_firstState)));
        }
    }
}