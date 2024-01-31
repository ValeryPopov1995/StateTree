using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Fire")]
    public class FireState : NpcState
    {
        [SerializeField] private Vector2Int _fireCount = new(3, 5);
        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _afterFire, _noAmmo, _noTarget;

        public override async Task<NodePort> Execute(Npc agent)
        {
            if (agent.TargetEnemy)
                agent.Mover.LookAt(new TransfromTarget(agent.TargetEnemy.transform));
            else
                return GetOutputPort(nameof(_noTarget));

            var weapon = agent.Inventory.TryGetItem<Weapon>();
            if (weapon)
            {
                int fireCount = Random.Range(_fireCount.x, _fireCount.y);
                for (int i = 0; i < fireCount; i++)
                {
                    if (!weapon.ConnectedMagazine.IsEmpty)
                        await weapon.Fire();
                    else
                        return GetOutputPort(nameof(_noAmmo));
                }
            }

            return GetOutputPort(nameof(_afterFire));
        }
    }
}
