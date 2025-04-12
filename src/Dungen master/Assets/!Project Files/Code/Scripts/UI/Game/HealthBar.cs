using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class HealthBar : BaseUI
    {
        [SerializeField] private Slider healthBar;

        public void SetHealthPercentage(float percentage)
        {
            healthBar.value = percentage;
        }
    }
}