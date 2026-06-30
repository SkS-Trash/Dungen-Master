using Core.Project.Dungeon;
using Core.Project.Home;
using Cysharp.Threading.Tasks;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project
{
    public class LaunchContinueGameState : IState, IEnterable
    {
        private readonly IProjectEngine _stateMachine;
        private readonly IProgressService _progress;

        public LaunchContinueGameState(
            IProjectEngine stateMachine,
            IProgressService progress
        )
        {
            _stateMachine = stateMachine;
            _progress = progress;
        }

        public UniTask OnEnterAsync(UnitEmpty _)
        {
            if (_progress.GlobalProgress.IsInDungeon)
            {
                _stateMachine.ChangeState<LaunchDungeonState>();
            }
            else
            {
                _stateMachine.ChangeState<HomeLoadState>();
            }

            return UniTask.CompletedTask;
        }
    }
}