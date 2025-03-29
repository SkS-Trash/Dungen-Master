using Core.Project.Base;
using Core.Project.Dungeon;
using Core.Project.Initialization;
using Core.Project.MainMenu;
using Factories.GameObject;
using Factories.UI;
using Observers.Input;
using Observers.UnityGameLoop;
using Providers.Assets;
using Providers.Containers.Scene;
using Providers.Data;
using Services.CoroutineRunner;
using Services.Progress;
using Services.ProjectManager;
using Services.SaveLoadData;
using Services.SceneLoader;
using Services.Window;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
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
            builder.Register<StateMachines.DirectControlMultiLayer.IStateMachine, StateMachines.DirectControlMultiLayer.StateMachine>(Lifetime.Transient);

            builder.Register<StateMachines.TransitionMultiLayer.IStateMachine, StateMachines.TransitionMultiLayer.StateMachine>(Lifetime.Transient);
            
            // States
            
            builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();
            builder.Register<ExitFromApplicationState>(Lifetime.Transient).AsSelf();

            builder.Register<InitializationState>(Lifetime.Transient).AsSelf();
            builder.Register<LoadEmptySceneState>(Lifetime.Transient).AsSelf();
            builder.Register<LoadingBasicResourcesState>(Lifetime.Transient).AsSelf();
            builder.Register<LoadProgressState>(Lifetime.Transient).AsSelf();
            builder.Register<OpenLoadingScreenState>(Lifetime.Transient).AsSelf();

            builder.Register<MainMenuState>(Lifetime.Transient).AsSelf();
            
            builder.Register<TestState>(Lifetime.Transient).AsSelf();
            builder.Register<GenerateMapState>(Lifetime.Transient).AsSelf();
            builder.Register<ConstructionMapState>(Lifetime.Transient).AsSelf();
            builder.Register<ConstructionDecorState>(Lifetime.Transient).AsSelf();
            builder.Register<ConstructionEnemyState>(Lifetime.Transient).AsSelf();
            builder.Register<InstantiateUIState>(Lifetime.Transient).AsSelf();
        }
    }
}