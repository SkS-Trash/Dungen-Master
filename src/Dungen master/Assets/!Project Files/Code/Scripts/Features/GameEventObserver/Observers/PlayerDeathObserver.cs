using Core.Project.MainMenu;
using Services.ProjectManager;
using VContainer;

namespace GameEventObserver
{
    public class PlayerDeathObserver : GameEventObserverBehaviour,
        IPlayerDeathSubscriber
    {
        private IProjectEngine _projectEngine;

        [Inject]
        public void Construct(IProjectEngine projectEngine)
        {
            _projectEngine = projectEngine;
        }

        protected override void Subscribe()
        {
            EventBus.Subscribe(this);
        }

        protected override void Unsubscribe()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnPlayerDeath()
        {
            _projectEngine.ChangeState<MainMenuState>();
        }
    }
}