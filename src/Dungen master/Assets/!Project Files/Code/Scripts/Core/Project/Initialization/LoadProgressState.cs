using Cysharp.Threading.Tasks;
using Infrastructure.Services.Progress;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Initialization
{
    public class LoadProgressState : IStateOneShot
    {
        private readonly IProgressService _progressService;

        public LoadProgressState(
            IProgressService progressService
        )
        {
            _progressService = progressService;
        }

        public UniTask OnEnterAsync(Unit _)
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