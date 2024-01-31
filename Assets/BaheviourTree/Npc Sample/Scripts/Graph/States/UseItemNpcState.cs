using System;
using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class UseItemNpcState : NpcState
    {
        [Item, SerializeField] private string _item;

        [SerializeField] private bool _fromInventory = true, _fromTeammate = true, _fromFloor = true;

        protected Item GetFromAnywhere(Npc npc, bool fromInventory = true, bool fromTeammate = true, bool fromFloor = true)
        {
            Item item = null;
            if (fromInventory)
                item = GetFromInventory(npc);
            else if (!item && fromTeammate)
                item = GetFromTeammate(npc);
            else if (!item && fromFloor)
                item = GetFromFloor(npc);

            return item;
        }

        protected Item GetFromInventory(Npc npc)
        {
            if (_fromInventory) return null;

            return npc.Inventory.TryGetItem(_item);
        }

        protected Item GetFromTeammate(Npc npc)
        {
            if (_fromTeammate) return null;

            var teammate = npc.TeamTag.GetNearestTeammate(npc);
            return teammate.Inventory.TryGetItem(_item);
        }

        protected Item GetFromFloor(Npc npc)
        {
            if (_fromFloor) return null;

            var items = FindObjectsByType<Item>(FindObjectsSortMode.None)
                .Where(item => item.IsOnFloor && item.GetType() == Type.GetType(_item));

            if (items.Count() == 0)
                return null;
            else if (items.Count() == 1)
                return items.FirstOrDefault();
            else
                return items.OrderBy(item => Vector3.Distance(npc.transform.position, item.transform.position)).FirstOrDefault();
        }
    }
}
