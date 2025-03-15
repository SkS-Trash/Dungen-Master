using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Initialization
{
    public class LoadProgressState : IState
    {
        private readonly IProgressService _progressService;
        public bool IsReusable => true;

        public LoadProgressState(
            IProgressService progressService
        )
        {
            _progressService = progressService;
        }

        public UniTask Initialize()
        {
            LoadProgress();

            return UniTask.CompletedTask;
        }

        private void LoadProgress()
        {
            _progressService.LoadProgress();
        }
    }
}