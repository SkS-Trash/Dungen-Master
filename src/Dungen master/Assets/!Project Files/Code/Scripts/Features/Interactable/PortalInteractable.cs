using Core.Project.Dungeon;
using Services.ProjectManager;
using VContainer;

namespace Interactable
{
    public class PortalInteractable : InteractableBase
    {
        private IProjectEngine _projectEngine;

        [Inject]
        public void Construct(IProjectEngine projectEngine)
        {
            _projectEngine = projectEngine;
        }

        public override void OnInteract()
        {
            _projectEngine.ChangeState<FirstLaunchDungeonState>();
        }
    }
}