using Providers.Containers.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class SceneInstaller : LifetimeScope
    {
        [SerializeReference] private ISceneContainer sceneContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            Container.Resolve<ISceneContainerProvider>().Set(sceneContainer);

            BindLevelServices();
        }

        private void BindLevelServices()
        {
        }
    }
}