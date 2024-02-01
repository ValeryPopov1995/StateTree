using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class UseItemNpcState : NpcState
    {
        [Item, SerializeField] private string _item;
        [SerializeField] private bool _fromInventory = true, _fromTeammate = false, _fromFloor = false;

        protected async Task<Item> GetFromAnywhere(Npc agent, bool fromInventory = true, bool fromTeammate = true, bool fromFloor = true)
        {
            Item item = null;
            if (fromInventory)
                item = await GetFromInventory(agent);
            else if (!item && fromTeammate)
                item = await GetFromTeammate(agent);
            else if (!item && fromFloor)
                item = await GetFromFloor(agent);

            return item;
        }

        protected async Task<Item> GetFromInventory(Npc agent)
        {
            await Task.Yield();
            if (!_fromInventory) return null;

            var item = agent.Inventory.TryGetItem(_item);
            if (item)
                WorldLog.Log(item.transform.position, "find in inventory" + item.name, agent);
            return item;
        }

        protected async Task<Item> GetFromTeammate(Npc agent)
        {
            await Task.Yield();
            if (!_fromTeammate) return null;

            var teammate = agent.TeamTag.GetNearestTeammate(agent);
            var item = teammate.Inventory.TryGetItem(_item);

            if (item)
            {
                WorldLog.Log(item.transform.position, "find from teammate" + item.name, agent);
                await MoveToPickUpItem(agent, item);
            }

            return item;
        }

        protected async Task<Item> GetFromFloor(Npc agent)
        {
            await Task.Yield();
            if (!_fromFloor) return null;

            var items = FindObjectsByType<Item>(FindObjectsSortMode.None)
                .Where(item => item.IsPickable
                && item.IsOnFloor
                && item.GetType() == Type.GetType(_item));

            if (items.Count() == 0)
                return null;

            var item = items
                    .OrderBy(item => Vector3.Distance(agent.transform.position, item.transform.position))
                    .FirstOrDefault();

            WorldLog.Log(item.transform.position, "find on floor" + item.name, agent);
            await MoveToPickUpItem(agent, item);
            return item;
        }

        private async Task MoveToPickUpItem(Npc agent, Item item)
        {
            if (item)
            {
                agent.Mover.MoveTo(new TransfromTarget(item.transform));
                await UniTask.WaitWhile(() => agent.Mover.IsMove);
                agent.Mover.MoveTo(new EmptyTarget());
                agent.Inventory.PickUp(item);
            }
        }
    }
}
