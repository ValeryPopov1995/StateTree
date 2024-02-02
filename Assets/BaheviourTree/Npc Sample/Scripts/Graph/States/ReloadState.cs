using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Reload")]
    public class ReloadState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _reloaded, _noAmmo;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            var weapon = agent.Inventory.TryGetItem<Weapon>();
            var newMagazine = agent.Inventory.Items
                .FirstOrDefault(item => item is Magazine && weapon.ConnectedMagazine != item) as Magazine;

            if (newMagazine)
            {
                await weapon.Reload(newMagazine);
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_reloaded)));
            }

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noAmmo)));
        }
    }
}
