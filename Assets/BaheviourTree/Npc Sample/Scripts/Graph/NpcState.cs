using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class NpcState : State<Npc>
    {
        [SerializeField, Input]
        private NpcState _input;

        [SerializeField] private bool _checkOrderAfterExecute = true;
        [SerializeField, Output]
        private NpcState _hasOrder;

        protected bool TryReactOnEnemy(Npc agent, int dangerDistance)
        {
            var dangerEnemies = agent.OverlapNpcs(dangerDistance)
                .Where(npc => npc.TeamTag != agent.TeamTag);
            var nearest = dangerEnemies.FirstOrDefault();

            if (nearest)
            {
                agent.TargetEnemy = nearest;
                return true;
            }

            return false;
        }

        /// <summary>
        /// If state is so long, use inside <see cref="State{TAgent}.Execute(TAgent)"/><code>
        /// if (agent.LastOrder != null)
        ///     return ReturnHaveOrderResult();
        /// </code>
        /// </summary>
        protected OutputPortStateResult<Npc> ReturnHaveOrderResult()
        {
            return new(GetOutputPort(nameof(_hasOrder)));
        }

        public override async Task<IStateResult<Npc>> Execute(Npc agent)
        {
            var result = await ExecuteNpcState(agent);

            if (_checkOrderAfterExecute && agent.OrderSystem.LastOrder != null)
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_hasOrder)));

            return result;
        }

        /// <summary>
        /// Decorator, <see cref="Npc"/> check <see cref="OrderSystem.LastOrder"/> != null after <see cref="State{TAgent}.Execute(TAgent)"/>
        /// </summary>
        /// <returns>result of state executing</returns>
        public abstract Task<IStateResult<Npc>> ExecuteNpcState(Npc agent);
    }
}