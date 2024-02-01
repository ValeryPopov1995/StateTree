using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Get Ammo")]
    public class GetAmmoState : UseItemNpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _collected, _noAmmo;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            var item = await GetFromAnywhere(agent, false);

            if (item)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_collected)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noAmmo)));
        }
    }
}
