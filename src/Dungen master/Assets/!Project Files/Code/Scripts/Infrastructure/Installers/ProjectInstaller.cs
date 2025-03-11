using Factories;
using Observers;
using Providers;
using Providers.Containers;
using Reflex.Core;
using Services;
using StateMachines.DirectControlMultiLayer;
using StateMachines.TransitionMultiLayer;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(typeof(GameObjectFactory), typeof(IGameObjectFactory));
            builder.AddSingleton(typeof(UIFactory), typeof(IUIFactory));

            builder.AddSingleton(typeof(UnityGameLoopObserver), typeof(IUnityGameLoopObserver));
            builder.AddSingleton(typeof(SceneContainerProvider), typeof(ISceneContainerProvider));
            builder.AddSingleton(typeof(StaticDataProvider), typeof(IStaticDataProvider));

            builder.AddSingleton(typeof(CoroutineRunner), typeof(ICoroutineRunner));
            builder.AddSingleton(typeof(ProgressInLocalStorageService), typeof(IProgressService));
            builder.AddSingleton(typeof(ProjectEngineAdapter), typeof(IProjectEngine));
            builder.AddSingleton(typeof(SaveLoadLocalDataService), typeof(ISaveLoadDataService));
            builder.AddSingleton(typeof(WindowService), typeof(IWindowService));

            builder.AddTransient(typeof(DirectControlMultiLayerStateMachine), typeof(IDirectControlMultiLayerStateMachine));
            builder.AddTransient(typeof(MultiLayerTransitionStateMachine), typeof(IMultiLayerTransitionStateMachine));

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