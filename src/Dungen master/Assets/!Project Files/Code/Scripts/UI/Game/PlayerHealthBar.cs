using Health;

namespace UI.Game
{
    public class PlayerHealthBar : HealthBar, IPlayerHealthPercentageSubscriber
    {
        public void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        public void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnPlayerHealthPercentageChanged(float percentage) => SetHealthPercentage(percentage);
    }
}