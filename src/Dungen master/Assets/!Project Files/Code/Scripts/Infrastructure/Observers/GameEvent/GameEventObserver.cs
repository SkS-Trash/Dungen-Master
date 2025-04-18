using System;
using Core.Project.MainMenu;
using Services.ProjectManager;
using Subscribers;
using Subscribers.EventBusSystem;

namespace Observers.GameEvent
{
    public class GameEventObserver : IGameEventObserver, IDisposable
    {
        private readonly IProjectEngine _projectEngine;

        public GameEventObserver(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;

            EventBus.Subscribe(this);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnPlayerDeath()
        {
            _projectEngine.ChangeState<MainMenuState>();
        }
    }

    public interface IGameEventObserver :
        IPlayerDeathSubscriber
    {
    }
}