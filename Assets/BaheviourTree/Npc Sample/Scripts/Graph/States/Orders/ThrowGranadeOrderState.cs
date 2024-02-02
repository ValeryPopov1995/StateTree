using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    /// <summary>
    /// Maske order to throw granade
    /// </summary>
    [CreateNodeMenu("StateTree/Npc Sample/Orders/ThrowGranade")]
    public class ThrowGranadeOrderState : MakeOrderNpcState<ThrowGranadeOrder>
    {
        [SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _ordered, _noEnemy;

        protected override async Task<IStateResult<Npc>> MakeOrder(Npc agent, IEnumerable<Npc> teammates, IEnumerable<Npc> enemies)
        {
            await Task.Yield();

            var teammate = teammates.FirstOrDefault();
            var enemy = enemies.FirstOrDefault();

            if (teammate && enemy)
            {
                teammate.OrderSystem.LastOrder = new ThrowGranadeOrder(enemy);
                agent.Communication.TellCommand(CommunicationCommandType.ThrowGranade);
                WorldLog.Log(teammate.transform.position, "order to throw granade", agent);
                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_ordered)));
            }

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noEnemy)));
        }
    }
}