using Cysharp.Threading.Tasks;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Dungeon
{
    public class FirstLaunchDungeonState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IProgressService _progress;

        public FirstLaunchDungeonState(
            IProjectEngine projectEngine,
            IProgressService progress
        )
        {
            _projectEngine = projectEngine;
            _progress = progress;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var gameProgress = _progress.GlobalProgress;
            gameProgress.currentLevelIndex = 0;
            _progress.SaveGlobal();

            var levelProgress = _progress.LevelProgress;
            levelProgress.currentLevelIndex = -1;
            _progress.SaveLevel();

            await _projectEngine.ChangeState<LaunchDungeonState>();
        }
    }
}