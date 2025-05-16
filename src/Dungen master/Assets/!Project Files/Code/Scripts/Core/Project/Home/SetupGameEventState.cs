using Cysharp.Threading.Tasks;
using Factories.GameEvent;
using GameEventObserver;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
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