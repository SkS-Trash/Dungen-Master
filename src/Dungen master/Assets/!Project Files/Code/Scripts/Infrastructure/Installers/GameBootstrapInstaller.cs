using Reflex.Core;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class GameBootstrapInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.OnContainerBuilt += OnBuilderOnOnContainerBuilt;
            builder.AddSingleton(typeof(GamingBootloader), typeof(IGamingInitializable));
            return;

            void OnBuilderOnOnContainerBuilt(Container container)
            {
                builder.OnContainerBuilt -= OnBuilderOnOnContainerBuilt;
                container.Resolve<IGamingInitializable>().Initialize();
            }
        }
    }
}