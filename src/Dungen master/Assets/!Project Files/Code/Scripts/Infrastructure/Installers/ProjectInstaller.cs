using System.Collections.Generic;
using System.Linq;
using Factories.GameEvent;
using Factories.GameObject;
using Factories.UI;
using Observers.Input;
using Observers.UnityGameLoop;
using Providers.Assets;
using Providers.Containers.Game;
using Providers.Containers.Scene;
using Providers.Data;
using Services.AudioPlayback;
using Services.CoroutineRunner;
using Services.CursorControl;
using Services.Progress;
using Services.ProjectManager;
using Services.SaveLoadData;
using Services.SceneLoader;
using Services.Window;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class ProjectInstaller : LifetimeScope
    {
        [SerializeField] private InputActionReader inputActionReader;
        [Space]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundEffectSource;
        [SerializeField] private AudioSource voiceOverSource;

        protected override void Configure(IContainerBuilder builder)
        {
            ForceResolveNonLazyBindings(builder);

            builder.Register<IGameObjectFactory, GameObjectFactory>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
            builder.Register<IGameEventFactory, GameEventFactory>(Lifetime.Singleton);

            builder.RegisterInstance(inputActionReader).AsImplementedInterfaces().AsSelf();
            builder.Register<IUnityGameLoopObserver, UnityGameLoopObserver>(Lifetime.Singleton);

            builder.Register<IGameContainerProvider, GameContainerProvider>(Lifetime.Singleton);
            builder.Register<ISceneContainerProvider, SceneContainerProvider>(Lifetime.Singleton);
            builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton);
            builder.Register<IAssetsProvider, AssetsAddressablesProvider>(Lifetime.Singleton);

            builder.Register<ICoroutineRunner, CoroutineRunner>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressInLocalStorageService>(Lifetime.Singleton);
            builder.Register<IProjectEngine, ProjectEngineAdapter>(Lifetime.Singleton);
            builder.Register<ISaveLoadDataService, SaveLoadLocalDataService>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IWindowService, WindowService>(Lifetime.Singleton);
            builder.Register<ICursorControlService, CursorControlService>(Lifetime.Singleton);
            builder.Register<IAudioPlaybackService, AudioPlaybackService>(Lifetime.Singleton)
                .WithParameter("musicSource", musicSource)
                .WithParameter("soundEffectSource", soundEffectSource)
                .WithParameter("voiceOverSource", voiceOverSource);

            builder.Register<StateMachines.DirectControlMultiLayer.IStatesFactory, StateMachines.DirectControlMultiLayer.StatesFactory>(Lifetime.Singleton);
            builder.Register<StateMachines.DirectControlMultiLayer.IStateMachine, StateMachines.DirectControlMultiLayer.StateMachine>(Lifetime.Transient);

            builder.Register<StateMachines.Transition.IStateMachine, StateMachines.Transition.StateMachine>(Lifetime.Transient);

            BindProjectStates(builder);
        }

        private static void ForceResolveNonLazyBindings(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                // Принудительно разрешить не ленивые зависимости
                container.Resolve<IEnumerable<INonLazy>>();
            });
        }

        private static void BindProjectStates(IContainerBuilder builder)
        {
            var states = typeof(StateMachines.DirectControlMultiLayer.IState)
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && typeof(StateMachines.DirectControlMultiLayer.IState).IsAssignableFrom(x));

            foreach (var state in states)
            {
                if (state.IsAbstract || state.IsInterface) 
                    continue;
                
                builder.Register(state, Lifetime.Transient).AsSelf();
            }
        }
    }
}