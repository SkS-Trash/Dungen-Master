using System;
using UnityEngine;

namespace Health
{
    public class HealthContainer : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;

        [field: SerializeField] public int CurrentHealth { get; protected set; }

        [field: SerializeField] public int MaxHealth { get; protected set; } = 100;

        private void Start()
        {
            SetCurrentHealth(CurrentHealth);
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

            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public virtual void SetCurrentHealth(int currentHealth)
        {
            CurrentHealth = currentHealth;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}