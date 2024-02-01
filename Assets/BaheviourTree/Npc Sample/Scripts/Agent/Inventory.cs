using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class Inventory : IInitializable, IDisposable
    {
        [SerializeField] private Transform _inventoryParent; // TODO use it only for Init()
        public List<Item> Items = new();

        public bool Initialized { get; private set; }



        public async void Init()
        {
            await Task.Yield();
            Items.ForEach(item => PickUpInternal(item));
            Initialized = true;
        }

        public void PickUp(Item item)
        {
            if (Items.Contains(item)) return;
            if (!item.IsPickable) return;
            Items.Add(item);
            PickUpInternal(item);
        }

        private void PickUpInternal(Item item)
        {
            if (item.IsInInventory)
                item.AttachedInventory.Drop(item);

            item.PickUpByInventory(this);
            item.transform.SetParent(_inventoryParent);
            item.AttachedInventory = this;
            item.Rigidbody.isKinematic = true;
        }

        /// <summary>
        /// Double-sided action to drop item from inventory. Also can use <see cref="Item.DropFromInventory"/>
        /// </summary>
        public void Drop(Item item)
        {
            if (!Items.Contains(item)) return;

            Items.Remove(item);
            item.transform.SetParent(null);
            item.AttachedInventory = null;
            item.Rigidbody.isKinematic = false;
        }

        internal TItem TryGetItem<TItem>() where TItem : Item
        {
            return Items.FirstOrDefault(item => item.GetType() == typeof(TItem)) as TItem;
        }

        internal Item TryGetItem(string itemType)
        {
            return Items.FirstOrDefault(item => item.GetType().AssemblyQualifiedName == itemType);
        }

        public void Dispose()
        {
            while (Items.Count > 0)
                Drop(Items[0]);
        }
    }
}
