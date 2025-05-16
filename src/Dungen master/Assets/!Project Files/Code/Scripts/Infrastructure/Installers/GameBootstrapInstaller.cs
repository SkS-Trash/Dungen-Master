using Core;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameBootstrapInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamingBootloader>();
        }
    }
}