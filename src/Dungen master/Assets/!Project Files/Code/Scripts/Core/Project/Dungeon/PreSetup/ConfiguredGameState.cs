using Cysharp.Threading.Tasks;
using Services.CursorControl;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConfiguredGameState : IStateOneShot
    {
        private readonly ICursorControlService _cursorControl;

        public ConfiguredGameState(
            ICursorControlService cursorControl
        )
        {
            _cursorControl = cursorControl;
        }

        public UniTask OnEnterAsync(UnitEmpty _)
        {
            _cursorControl.SetLock(CursorLockMode.Locked);
            _cursorControl.SetVisible(false);

            return UniTask.CompletedTask;
        }
    }
}