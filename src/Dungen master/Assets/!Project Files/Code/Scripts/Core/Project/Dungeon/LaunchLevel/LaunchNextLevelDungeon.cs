using Cysharp.Threading.Tasks;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Dungeon
{
    public class LaunchNextLevelDungeon : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IProgressService _progressService;

        public LaunchNextLevelDungeon(
            IProjectEngine projectEngine,
            IProgressService progressService
        )
        {
            _projectEngine = projectEngine;
            _progressService = progressService;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            _progressService.GlobalProgress.currentLevelIndex++;
            _progressService.SaveGlobal();

            await _projectEngine.ChangeState<LaunchDungeonState>();
        }
    }
}