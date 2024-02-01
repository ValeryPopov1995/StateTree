using System.Threading.Tasks;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Start/Interrupt")]
    public class NpcStartInterruptState : StartInterruptState<Npc>
    {
        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            await Task.Yield();
            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_firstState)));
        }
    }
}