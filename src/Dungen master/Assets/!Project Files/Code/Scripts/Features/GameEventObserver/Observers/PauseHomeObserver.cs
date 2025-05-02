using Core.Project.Home;
using Observers.Input;
using Services.ProjectManager;
using VContainer;

namespace GameEventObserver
{
    public class PauseHomeObserver : GameEventObserverBehaviour, IPauseGameSubscriber
    {
        private IInputActionReader _inputActionReader;
        private IProjectEngine _projectEngine;

        private bool _isPaused;

        [Inject]
        public void Construct(IInputActionReader inputActionReader, IProjectEngine projectEngine)
        {
            _inputActionReader = inputActionReader;
            _projectEngine = projectEngine;
        }

        protected override void Subscribe()
        {
            _inputActionReader.OnCancelChanged += OnPauseGameTrigger;
        }

        protected override void Unsubscribe()
        {
            _inputActionReader.OnCancelChanged -= OnPauseGameTrigger;
        }

        private void OnPauseGameTrigger()
        {
            if (!_isPaused)
            {
                _projectEngine.ChangeState<HomePauseState>();
            }
        }

        public void OnPauseGame(bool isPaused)
        {
            _isPaused = isPaused;
        }
    }
}