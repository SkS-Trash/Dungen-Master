using System.Linq;
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
            builder.Register<StateMachines.DirectControlMultiLayer.IStateMachine,
                StateMachines.DirectControlMultiLayer.StateMachine>(Lifetime.Transient);

            builder.Register<StateMachines.TransitionMultiLayer.IStateMachine,
                StateMachines.TransitionMultiLayer.StateMachine>(Lifetime.Transient);

            BindProjectStates(builder);
        }

        private static void BindProjectStates(IContainerBuilder builder)
        {
            var states = typeof(IState).Assembly.GetTypes().Where(x => x.IsClass && typeof(IState).IsAssignableFrom(x));
            foreach (var state in states)
            {
                if (state.IsAbstract || state.IsInterface) continue;
                builder.Register(state, Lifetime.Transient).AsSelf();
            }
        }
    }
}