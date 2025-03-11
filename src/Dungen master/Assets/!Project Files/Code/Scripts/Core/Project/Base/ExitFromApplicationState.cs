using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project
{
    public class ExitFromApplicationState : IState
    {
        public bool IsReusable => true;

        public UniTask Initialize()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}