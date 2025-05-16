using Health;
using UnityEngine;

namespace Enemy.Components.UI
{
    public class EnemyHealthBar : HealthBar
    {
        [SerializeField] private HealthContainer health;

        private void OnEnable()
        {
            SetHealthPercentage((float)health.CurrentHealth / health.MaxHealth);    
            health.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int obj)
        {
            SetHealthPercentage((float)health.CurrentHealth / health.MaxHealth);
        }
    }
}