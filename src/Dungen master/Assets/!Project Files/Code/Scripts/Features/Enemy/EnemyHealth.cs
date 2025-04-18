using System;
using Health;
using Sirenix.OdinInspector;

namespace Enemy
{
    public class EnemyHealth : HealthContainer
    {
        public event Action<int> OnHealthChanged;

        [field: ShowInInspector, HideInEditorMode]
        public bool WasDamaged { get; set; }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);

            OnHealthChanged?.Invoke(CurrentHealth);

            WasDamaged = true;
        }

        public override void Heal(int amount)
        {
            base.Heal(amount);

            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}