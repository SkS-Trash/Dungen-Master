using UI;
using UI.Game;
using UnityEngine;

namespace Player
{
    public class HudUI : ElementUI
    {
        [field: SerializeField] public PlayerHealthBar PlayerHealthBar { get; private set; }
        [field: SerializeField] public MagicCooldown MagicCooldown { get; private set; }
    }
}