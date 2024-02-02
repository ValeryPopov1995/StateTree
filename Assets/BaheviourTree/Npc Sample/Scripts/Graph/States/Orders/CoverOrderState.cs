using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Orders/Cover")]
    public class CoverOrderState : MakeOrderNpcState<ThrowGranadeOrder>
    {
        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _ordered, _failed;

        protected override async Task<IStateResult<Npc>> MakeOrder(Npc agent, IEnumerable<Npc> teammates, IEnumerable<Npc> enemies)
        {
            await Task.Yield();
            
            var teammate = teammates.FirstOrDefault();
            var enemy = enemies.FirstOrDefault();

            throw new System.NotImplementedException();
        }
    }
}