using Cysharp.Threading.Tasks;
using Services.CursorControl;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Home
{
    public class ConfiguredHomeState : IStateOneShot
    {
        private readonly ICursorControlService _cursorControl;

        public ConfiguredHomeState(
            ICursorControlService cursorControl
        )
        {
            _cursorControl = cursorControl;
        }

        public UniTask OnEnterAsync(Unit _)
        {
            CursorLock();

            return UniTask.CompletedTask;
        }

        private void CursorLock()
        {
            _cursorControl.SetLock(CursorLockMode.Locked);
            _cursorControl.SetVisible(false);
        }
    }
}