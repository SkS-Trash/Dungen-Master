using Health;
using Progress;
using Services.Progress;

namespace Player
{
    public class PlayerHealth : HealthContainer,
        ILevelProgressLoadSubscriber, ILevelProgressCollector
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Subscribe(this);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            OnHealthChanged();

            if (CurrentHealth <= 0)
            {
                EventBus.RaiseEvent<IPlayerDeathSubscriber>(subscriber => subscriber.OnPlayerDeath());
            }
        }

        public override void Heal(int healAmount)
        {
            base.Heal(healAmount);

            OnHealthChanged();
        }

        private float GetPercentage() => (float)CurrentHealth / MaxHealth;

        public void OnProgressLoaded(LevelSaveData progress)
        {
            CurrentHealth = progress.player.health;

            OnHealthChanged();
        }

        public void Collect(LevelSaveData target)
        {
            target.player.health = CurrentHealth;
        }

        private void OnHealthChanged()
        {
            EventBus.RaiseEvent<IPlayerHealthPercentageSubscriber>(subscriber =>
                subscriber.OnPlayerHealthPercentageChanged(GetPercentage()));
        }
    }
}