using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthBar : ElementUI
    {
        [SerializeField] private Slider healthBar;

        public void SetHealthPercentage(float percentage)
        {
            healthBar.value = percentage;
        }
    }
}