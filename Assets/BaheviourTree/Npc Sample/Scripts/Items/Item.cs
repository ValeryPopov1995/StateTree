using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class Item : MonoBehaviour
    {
        public bool IsInInventory => AttachedInventory != null;
        public bool IsOnFloor => !IsInInventory;
        public bool IsPickable { get; internal set; } = true;
        public Rigidbody Rigidbody { get; private set; }
        public Inventory AttachedInventory { get; internal set; }

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void PickUpByInventory(Inventory inventory)
        {
            if (inventory == null)
                throw new System.Exception("no inventory to pick up");

            if (inventory == AttachedInventory)
            {
                Debug.LogWarning("same inventory to pick up");
                return;
            }

            if (!IsPickable) return;

            inventory.PickUp(this);
        }

        /// <summary>
        /// Double-sided action to drop item from inventory. Also can use <see cref="Inventory.Drop(Item)"/>
        /// </summary>
        public void DropFromInventory()
        {
            if (!IsInInventory) return;

            AttachedInventory.Drop(this);
        }

        public static Item TryFindOnFloor(Vector3 startPoint, float maxDistance, string itemType)
        {
            return Physics.OverlapSphere(startPoint, maxDistance)
                .Where(collider =>
                    collider.attachedRigidbody.TryGetComponent<Item>(out var item)
                    && item.GetType().AssemblyQualifiedName == itemType)
                .OrderBy(collider => Vector3.Distance(startPoint, collider.transform.position))
                .FirstOrDefault()
                .GetComponent<Item>();
        }
    }
}