using System;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class Health : IDamagable, IHealable
    {
        public Action OnDeath, OnDamage, OnHeal;

        public bool IsDead => Value <= 0;
        public bool IsAlive => !IsDead;
        [field: SerializeField, Min(0)] public int Value { get; private set; } = 10;
        public float Value01 => (float)Value / MaxValue;
        [field: SerializeField, Min(0)] public int MaxValue { get; private set; } = 10;

        public void GetHeal(int heal)
        {
            if (IsDead) return;

            if (heal <= 0) throw new ArgumentOutOfRangeException("heal value < 0");
            Value += heal;
            if (Value > MaxValue) Value = MaxValue;
            OnHeal?.Invoke();
        }

        public void GetDamage(int damage)
        {
            if (IsDead) return;

            if (damage <= 0)
                throw new ArgumentOutOfRangeException("damage value < 0");

            Value -= damage;
            if (Value < 0) Value = 0;

            OnDamage?.Invoke();

            if (Value == 0)
                OnDeath?.Invoke();
        }
    }
}
