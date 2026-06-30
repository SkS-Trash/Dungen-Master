using Health;
using Progress;
using Services.Progress;

namespace Player.Components
{
    public class PlayerHealth : HealthContainer, ILevelProgressCollector
    {
        private void OnEnable()
        {
            LevelProgressSaveCollectorsProvider.Instance.Subscribe(this);
            OnHealthChanged += OnDeathHandler;
        }

        private void OnDisable()
        {
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
            OnHealthChanged -= OnDeathHandler;
        }

        public void Collect(LevelSaveData target)
        {
            target.player.health = CurrentHealth;
        }

        private void OnDeathHandler(int _)
        {
            if (CurrentHealth <= 0)
            {
                EventBus.RaiseEvent<IPlayerDeathEvent>(e => e.OnPlayerDeath());
            }
        }
    }
}