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
        }

        private void OnDisable()
        {
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
        }

        public void Collect(LevelSaveData target)
        {
            target.player.health = CurrentHealth;
        }
    }
}