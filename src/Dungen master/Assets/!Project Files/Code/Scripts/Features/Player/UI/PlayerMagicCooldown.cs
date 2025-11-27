using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class PlayerMagicCooldown : ElementUI
    {
        [SerializeField] private Image magicCooldown;

        public void SetMagicCooldownPercentage(float percentage)
        {
            magicCooldown.fillAmount = percentage;
        }
    }
}