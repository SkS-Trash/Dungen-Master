using Observers.Input;
using VContainer;

namespace GameEventObserver
{
    public class StartPauseObserver : GameEventObserverBehaviour,
        IPauseGameSubscriber
    {
        public override GameEventType EventType => GameEventType.StartPause;

        private IInputActionReader _inputActionReader;

        private bool _isPaused;

        [Inject]
        public void Construct(IInputActionReader inputActionReader)
        {
            _inputActionReader = inputActionReader;
        }

        protected override void Subscribe()
        {
            EventBus.Subscribe(this);
            if (_inputActionReader != null)
                _inputActionReader.OnCancelChanged += OnPauseGameTrigger;
        }

        protected override void Unsubscribe()
        {
            EventBus.Unsubscribe(this);
            if (_inputActionReader != null)
                _inputActionReader.OnCancelChanged -= OnPauseGameTrigger;
        }

        private void OnPauseGameTrigger()
        {
            if (!_isPaused)
            {
                Notify();
            }
        }

        public void OnPauseGame(bool isPaused)
        {
            _isPaused = isPaused;
        }
    }
}