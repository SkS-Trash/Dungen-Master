using Sirenix.OdinInspector;
using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField, ReadOnly, HideInEditorMode]
        private int maxHealth = 100;

        [SerializeField, ReadOnly, HideInEditorMode]
        private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                EventBus.RaiseEvent<IPlayerDeathSubscriber>(subscriber => subscriber.OnPlayerDeath());
            }

            EventBus.RaiseEvent<IPlayerHealthPercentageSubscriber>(subscriber =>
                subscriber.OnPlayerHealthPercentageChanged(GetPercentage()));
        }

        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            EventBus.RaiseEvent<IPlayerHealthPercentageSubscriber>(subscriber =>
                subscriber.OnPlayerHealthPercentageChanged(GetPercentage()));
        }

        private float GetPercentage() => (float)currentHealth / maxHealth;
    }
}