using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class MakeOrderNpcState<T> : NpcState where T : Order
    {
        [SerializeField] private int _findNpcsDistance = 9;

        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _cantOrder;

        public override async Task<IStateResult<Npc>> ExecuteNpcState(Npc agent)
        {
            if (!agent.OrderSystem.CanOrder)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_cantOrder)));

            var npcs = agent.OverlapNpcs(_findNpcsDistance)
                .OrderBy(npc => Vector3.Distance(agent.transform.position, npc.transform.position))
                .ToArray();
            var teammates = npcs.Where(npc => npc.TeamTag == agent.TeamTag && npc.OrderSystem.CanPerform)
                .ToArray();
            var enemies = npcs.Where(npc => npc.TeamTag != agent.TeamTag)
                .ToArray();

            return await MakeOrder(agent, teammates, enemies);
        }

        protected abstract Task<IStateResult<Npc>> MakeOrder(Npc agent, IEnumerable<Npc> teammates, IEnumerable<Npc> enemies);
    }
}