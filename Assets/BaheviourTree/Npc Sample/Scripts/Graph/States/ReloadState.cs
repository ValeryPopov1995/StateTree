using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Reload")]
    public class ReloadState : NpcState
    {
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _reloaded, _noAmmo;

        public override async Task<NodePort> Execute(Npc agent)
        {
            var weapon = agent.Inventory.TryGetItem<Weapon>();
            var newMagazine = agent.Inventory.Items
                .FirstOrDefault(item => item is Magazine && weapon.ConnectedMagazine != item) as Magazine;

            if (newMagazine)
            {
                await weapon.Reload(newMagazine);
                return GetOutputPort(nameof(_reloaded));
            }

            return GetOutputPort(nameof(_noAmmo));
        }
    }
}
