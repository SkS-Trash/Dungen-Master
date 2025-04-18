using Sirenix.OdinInspector;
using UnityEngine;

namespace Health
{
    public class HealthContainer : MonoBehaviour
    {
        [field: ShowInInspector, ReadOnly, HideInEditorMode]
        public int CurrentHealth { get; private set; }

        [field: SerializeField] 
        public int MaxHealth { get; private set; } = 100;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
            }
        }

        public virtual void Heal(int healAmount)
        {
            CurrentHealth += healAmount;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
}