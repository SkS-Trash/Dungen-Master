using Core.Project.Base;
using Core.Project.Initialization;
using Core.Project.MainMenu;
using Factories;
using Infrastructure.Observers.Input;
using Observers;
using Providers;
using Providers.Containers;
using Services;
using StateMachines.DirectControlMultiLayer;
using StateMachines.TransitionMultiLayer;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using IState = StateMachines.DirectControlMultiLayer.IState;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : LifetimeScope
    {
        [SerializeField] private InputActionReader inputActionReader;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IGameObjectFactory, GameObjectFactory>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);

            builder.RegisterInstance(inputActionReader).AsImplementedInterfaces().AsSelf();
            builder.Register<IUnityGameLoopObserver, UnityGameLoopObserver>(Lifetime.Singleton);

            builder.Register<ISceneContainerProvider, SceneContainerProvider>(Lifetime.Singleton);
            builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton);
            builder.Register<IAssetsProvider, AssetsAddressablesProvider>(Lifetime.Singleton);

            builder.Register<ICoroutineRunner, CoroutineRunner>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressInLocalStorageService>(Lifetime.Singleton);
            builder.Register<IProjectEngine, ProjectEngineAdapter>(Lifetime.Singleton);
            builder.Register<ISaveLoadDataService, SaveLoadLocalDataService>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IWindowService, WindowService>(Lifetime.Singleton);

            builder.Register<IStatesFactory, StatesFactory>(Lifetime.Singleton);
            builder.Register<IDirectControlMultiLayerStateMachine, DirectControlMultiLayerStateMachine>(Lifetime.Transient);

            builder.Register<IMultiLayerTransitionStateMachine, MultiLayerTransitionStateMachine>(Lifetime.Transient);
            
            // States
            
            builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();;
            builder.Register<ExitFromApplicationState>(Lifetime.Transient).AsSelf();;

            builder.Register<InitializationState>(Lifetime.Transient).AsSelf();;
            builder.Register<LoadEmptySceneState>(Lifetime.Transient).AsSelf();;
            builder.Register<LoadingBasicResourcesState>(Lifetime.Transient).AsSelf();;
            builder.Register<LoadProgressState>(Lifetime.Transient).AsSelf();;
            builder.Register<OpenLoadingScreenState>(Lifetime.Transient).AsSelf();;

            builder.Register<MainMenuState>(Lifetime.Transient).AsSelf();;
        }
    }
}