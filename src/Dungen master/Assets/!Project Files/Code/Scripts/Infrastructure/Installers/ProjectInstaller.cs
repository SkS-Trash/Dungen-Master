using Factories;
using Infrastructure.Observers.Input;
using Observers;
using Providers;
using Providers.Containers;
using Reflex.Core;
using Reflex.Extensions;
using Services;
using StateMachines.DirectControlMultiLayer;
using StateMachines.TransitionMultiLayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Installers
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private InputActionReader inputActionReader;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(typeof(GameObjectFactory), typeof(IGameObjectFactory));
            builder.AddSingleton(typeof(UIFactory), typeof(IUIFactory));

            builder.AddSingleton(typeof(InputActionReader), typeof(IInputActionReader));
            builder.AddSingleton(typeof(UnityGameLoopObserver), typeof(IUnityGameLoopObserver));

            builder.AddSingleton(typeof(SceneContainerProvider), typeof(ISceneContainerProvider));
            builder.AddSingleton(typeof(StaticDataProvider), typeof(IStaticDataProvider));
            builder.AddSingleton(typeof(AssetsAddressablesProvider), typeof(IAssetsProvider));

            builder.AddSingleton(typeof(CoroutineRunner), typeof(ICoroutineRunner));
            builder.AddSingleton(typeof(ProgressInLocalStorageService), typeof(IProgressService));
            builder.AddSingleton(typeof(ProjectEngineAdapter), typeof(IProjectEngine));
            builder.AddSingleton(typeof(SaveLoadLocalDataService), typeof(ISaveLoadDataService));
            builder.AddSingleton(typeof(SceneLoaderService), typeof(ISceneLoaderService));
            builder.AddSingleton(typeof(WindowService), typeof(IWindowService));

            builder.AddSingleton(typeof(StatesFactory), typeof(IStatesFactory));
            builder.AddTransient(typeof(DirectControlMultiLayerStateMachine), typeof(IDirectControlMultiLayerStateMachine));

            builder.AddTransient(typeof(MultiLayerTransitionStateMachine), typeof(IMultiLayerTransitionStateMachine));
        }
    }
}