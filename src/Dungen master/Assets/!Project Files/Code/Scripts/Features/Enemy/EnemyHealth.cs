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

        [field: SerializeField, ReadOnly] public int CurrentHealth { get; private set; }

        [SerializeField, HideInEditorMode] private int maxHealth = 100;

        private void Start()
        {
            CurrentHealth = maxHealth;
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
            if (CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}