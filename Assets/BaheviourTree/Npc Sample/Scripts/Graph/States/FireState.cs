using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Fire")]
    public class FireState : NpcState
    {
        [SerializeField] private Vector2Int _fireCount = new(3, 5);
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _afterFire, _noAmmoInWeapon, _noTarget;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            if (agent.TargetEnemy)
                agent.Mover.LookAt(new TransfromTarget(agent.TargetEnemy.transform));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noTarget)));

            var weapon = agent.Inventory.TryGetItem<Weapon>();
            if (weapon)
            {
                int fireCount = Random.Range(_fireCount.x, _fireCount.y);
                for (int i = 0; i < fireCount; i++)
                {
                    if (!weapon.ConnectedMagazine.IsEmpty)
                        await weapon.Fire();
                    else
                        return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noAmmoInWeapon))); // no ammo diring fire
                }

                if (weapon.ConnectedMagazine.IsEmpty)
                    return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noAmmoInWeapon))); // no ammo after fire
            }

            if (agent.TargetEnemy)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_afterFire)));
            else
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noTarget)));
        }
    }
}
