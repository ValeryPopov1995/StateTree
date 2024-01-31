using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public abstract class Item : MonoBehaviour
    {
        public bool IsInInventory => AttachedInventory != null;
        public bool IsOnFloor => !IsInInventory;
        public Rigidbody Rigidbody { get; private set; }
        public Inventory AttachedInventory { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void PickUpByInventory(Inventory inventory)
        {
            if (AttachedInventory != null || inventory == null)
                throw new System.Exception("no inventory to pick up or item was picked up");

            AttachedInventory = inventory;
            Rigidbody.isKinematic = true;
        }

        /// <summary>
        /// Double-sided action to drop item from inventory. Also can use <see cref="Inventory.Drop(Item)"/>
        /// </summary>
        public void DropFromInventory()
        {
            if (AttachedInventory == null)
                throw new System.Exception("no inventory to drop");

            AttachedInventory.Drop(this);
            AttachedInventory = null;
            Rigidbody.isKinematic = false;
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