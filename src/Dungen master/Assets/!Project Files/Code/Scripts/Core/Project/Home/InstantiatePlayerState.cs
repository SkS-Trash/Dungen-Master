using Cysharp.Threading.Tasks;
using Factories.GameObject;
using StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Home
{
    public class InstantiatePlayerState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await InstantiatePlayer();
        }

        private async UniTask InstantiatePlayer()
        {
            await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.PLAYER);
        }
    }
}