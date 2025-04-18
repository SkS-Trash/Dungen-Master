using Health;
using Subscribers;
using Subscribers.EventBusSystem;

namespace Player
{
    public class PlayerHealth : HealthContainer
    {
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            EventBus.RaiseEvent<IPlayerHealthPercentageSubscriber>(subscriber =>
                subscriber.OnPlayerHealthPercentageChanged(GetPercentage()));

            if (CurrentHealth <= 0)
            {
                EventBus.RaiseEvent<IPlayerDeathSubscriber>(subscriber => subscriber.OnPlayerDeath());
            }
        }

        public override void Heal(int healAmount)
        {
            base.Heal(healAmount);

            EventBus.RaiseEvent<IPlayerHealthPercentageSubscriber>(subscriber =>
                subscriber.OnPlayerHealthPercentageChanged(GetPercentage()));
        }

        private float GetPercentage() => (float)CurrentHealth / MaxHealth;
    }
}