using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;
using UnityEngine.AddressableAssets;

namespace Core.Project.Initialization
{
    public class LoadingBasicResourcesState : IState, IEnterable
    {
        private const string DEFAULT_ADDRESSABLE_LABEL = "default";

        public async UniTask OnEnterAsync(Unit _)
        {
            await AddressablesInitialize();

            await LoadDefaultAssets();
        }

        private static async UniTask LoadDefaultAssets()
        {
            await Addressables.LoadResourceLocationsAsync(DEFAULT_ADDRESSABLE_LABEL);
        }

        private static async UniTask AddressablesInitialize()
        {
            await Addressables.InitializeAsync();
        }
    }
}