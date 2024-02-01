using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class NpcState : State<Npc>
    {
        [field: SerializeField, Input]
        private NpcState _input;

        protected bool TryReactOnEnemy(Npc agent, int dangerDistance)
        {
            var dangerEnemies = agent.OverlapNpcs(dangerDistance)
                .Where(npc => npc.TeamTag != agent.TeamTag);
            var nearest = dangerEnemies.FirstOrDefault();

            if (nearest)
            {
                agent.TargetEnemy = nearest;
                WorldLog.Log(nearest.transform.position, "danger", agent);
                return true;
            }

            return false;
        }
    }
}