using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConfiguredGameState : IStateOneShot
    {
        public UniTask OnEnterAsync(Unit _)
        {
            CursorLock();

            return UniTask.CompletedTask;
        }

        private void CursorLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}