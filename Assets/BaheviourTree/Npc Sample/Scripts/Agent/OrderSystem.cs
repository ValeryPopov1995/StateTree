using System;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class OrderSystem : IDisposable
    {
        [field: SerializeField] public bool CanOrder { get; internal set; }
        [field: SerializeField] public bool CanPerform { get; internal set; }
        public Order LastOrder
        {
            get
            {
                if (CanPerform)
                    return _lastOrder;

                return _lastOrder = null;
            }
            set
            {
                if (CanPerform)
                    _lastOrder = value;
            }
        }
        private Order _lastOrder;

        public void Dispose()
        {
            CanOrder = false;
            CanPerform = false;
            LastOrder = null;
        }
    }
}