using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Base
{
    public class ExitFromApplicationState : IStateOneShot
    {
        public UniTask OnEnterAsync(Unit _)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif

            return UniTask.CompletedTask;
        }
    }
}