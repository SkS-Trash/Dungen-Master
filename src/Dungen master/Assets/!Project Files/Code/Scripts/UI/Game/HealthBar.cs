using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class HealthBar : BaseUI, IPlayerHealthPercentageSubscriber
    {
        [SerializeField] private Slider healthBar;

        public void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        public void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void SetHealthPercentage(float percentage)
        {
            healthBar.value = percentage;
        }

        public void OnPlayerHealthPercentageChanged(float percentage) => SetHealthPercentage(percentage);
    }
}