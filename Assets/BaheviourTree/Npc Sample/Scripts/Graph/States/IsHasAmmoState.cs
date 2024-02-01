using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Has Ammo")]
    public class IsHasAmmoState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _hasAmmo, _noAmmo;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            if (agent.Inventory.TryGetItem<Magazine>())
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasAmmo)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noAmmo)));
        }
    }
}
