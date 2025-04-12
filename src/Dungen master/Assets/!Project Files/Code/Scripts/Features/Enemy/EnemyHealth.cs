using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;

        [field: ShowInInspector, HideInEditorMode]
        public bool WasDamaged { get; set; }

        [field: ShowInInspector, ReadOnly, HideInEditorMode]
        public int CurrentHealth { get; private set; }

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            WasDamaged = true;

            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
            }

            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public void Heal(int healAmount)
        {
            CurrentHealth += healAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}