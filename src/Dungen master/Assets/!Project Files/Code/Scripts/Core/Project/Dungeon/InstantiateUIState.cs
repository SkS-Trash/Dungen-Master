using Cysharp.Threading.Tasks;
using Services.Window;
using StateMachines.DirectControlMultiLayer;
using UI.Game.Player;

namespace Core.Project.Dungeon
{
    public class InstantiateUIState : IStateOneShot
    {
        private readonly IWindowService _windowService;

        public InstantiateUIState(
            IWindowService windowService
        )
        {
            _windowService = windowService;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await InstantiateHudUI();
            SetupHudUI();
        }

        private async UniTask InstantiateHudUI()
        {
            await _windowService.OpenAndGet<HudUI>(WindowID.HUD);
        }

        private void SetupHudUI()
        {
            var hudUI = _windowService.Get<HudUI>(WindowID.HUD);
            hudUI.PlayerHealthBar.SetHealthPercentage(1f);
            hudUI.MagicCooldown.SetMagicCooldownPercentage(0f);
        }
    }
}