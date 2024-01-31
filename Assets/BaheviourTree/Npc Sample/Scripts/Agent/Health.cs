using System;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class Health
    {
        public Action OnDeath, OnDamage, OnHeal;

        public bool IsDead => Value <= 0;
        public bool IsAlive => !IsDead;
        [field: SerializeField, Min(0)] public int Value { get; private set; } = 10;
        [field: SerializeField, Min(0)] public int MaxValue { get; private set; } = 10;

        public void Heal(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException();
            Value += value;
            if (Value > MaxValue) Value = MaxValue;
            OnHeal?.Invoke();
        }

        public void Damage(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException();
            Value -= value;
            if (Value < 0) Value = 0;

            OnDamage?.Invoke();

            if (Value == 0)
                OnDeath?.Invoke();
        }
    }
}
