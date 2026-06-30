using System;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Health
{
    public class HealthContainer : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;

        public ReactiveProperty<float> HealthPercentage { get; } = new(1f);

        [field: ShowInInspector, ReadOnly] public int CurrentHealth { get; protected set; }
        [field: ShowInInspector, ReadOnly] public int MaxHealth { get; protected set; } = 100;

        private void Start()
        {
            SetCurrentHealth(MaxHealth);
        }

        public virtual void TakeDamage(int damage)
        {
            SetCurrentHealth(CurrentHealth - damage);
        }

        public virtual void Heal(int healAmount)
        {
            SetCurrentHealth(CurrentHealth + healAmount);
        }

        public virtual void SetMaxHealth(int maxHealth, bool resetCurrentHealth = true)
        {
            MaxHealth = maxHealth;

            if (resetCurrentHealth || CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }

            NotifyChanged();
        }

        public virtual void SetCurrentHealth(int currentHealth)
        {
            CurrentHealth = currentHealth;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            NotifyChanged();
        }

        protected virtual void NotifyChanged()
        {
            OnHealthChanged?.Invoke(CurrentHealth);

            HealthPercentage.Value = (float)CurrentHealth / MaxHealth;
        }
    }
}