using UnityEngine;

namespace UI.Game
{
    public class HudUI : BaseUI
    {
        [field: SerializeField] public HealthBar HealthBar { get; private set; }
        [field: SerializeField] public MagicCooldown MagicCooldown { get; private set; }
    }
}