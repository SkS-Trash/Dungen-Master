using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class MagicCooldown : BaseUI, IPlayerMagicCooldownSubscriber
    {
        [SerializeField] private Image magicCooldown;

        public void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        public void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void SetMagicCooldownPercentage(float percentage)
        {
            magicCooldown.fillAmount = percentage;
        }

        public void OnPlayerMagicCooldownChanged(float percentage) => SetMagicCooldownPercentage(percentage);
    }
}