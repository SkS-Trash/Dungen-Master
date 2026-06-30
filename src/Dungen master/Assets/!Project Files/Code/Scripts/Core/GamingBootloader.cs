using Core.Project;
using Services.ProjectManager;
using VContainer.Unity;

namespace Core
{
    /// <summary>
    /// Этот класс отвечает за начальную загрузку игры.
    /// </summary>
    public class GamingBootloader : IStartable
    {
        private readonly IProjectEngine _projectEngine;

        public GamingBootloader(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public void Start()
        {
            _projectEngine.ChangeState<BootstrapState>();
        }
    }
}