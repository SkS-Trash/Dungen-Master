using Cysharp.Threading.Tasks;
using Factories.GameEvent;
using GameEventObserver;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
    public class HomeLoadState : IState, IEnterable, IExitable
    {
        private readonly IProjectEngine _projectEngine;

        public HomeLoadState(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await _projectEngine.RunOneShot<LoadHomeSceneState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<SetupGameEventState>();

            await _projectEngine.ChangeState<HomeState>();
        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }

    public class SetupGameEventState : IStateOneShot
    {
        private readonly IGameEventFactory _gameEventFactory;
        private readonly IProjectEngine _projectEngine;

        public SetupGameEventState(
            IGameEventFactory gameEventFactory,
            IProjectEngine projectEngine
        )
        {
            _gameEventFactory = gameEventFactory;
            _projectEngine = projectEngine;
        }

        public UniTask OnEnterAsync(Unit _)
        {
            SetupStartPauseEvent();

            return UniTask.CompletedTask;
        }

        private void SetupStartPauseEvent()
        {
            var @event = _gameEventFactory.CreateGameEvent(GameEventType.StartPause);
            @event.Register(OnStartPauseEvent);

            void OnStartPauseEvent(GameEventObserverBehaviour gameEventObserverBehaviour)
            {
                _projectEngine.RunOneShot<HomePauseState>();
            }
        }
    }
}