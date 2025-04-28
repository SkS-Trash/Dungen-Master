using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Settings
{
    public class SettingsState : IState, IEnterable, IExitable
    {
        private readonly IProjectEngine _projectEngine;

        public SettingsState(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public UniTask OnEnterAsync(Unit _)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}