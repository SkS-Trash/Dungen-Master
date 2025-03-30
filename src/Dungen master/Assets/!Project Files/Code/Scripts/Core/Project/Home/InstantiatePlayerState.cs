using Cysharp.Threading.Tasks;
using Factories.GameObject;
using Providers.Containers.Scene;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Home
{
    public class InstantiatePlayerState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly ISceneContainer _sceneContainer;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory,
            ISceneContainerProvider sceneContainerProvider
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _sceneContainer = sceneContainerProvider.Get();
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await InstantiatePlayer();
        }

        private async UniTask InstantiatePlayer()
        {
            if (_sceneContainer is not IPlayerSpawnData playerSpawnData)
            {
                Debug.LogError("Данные Player Spawn не установлены в контейнере сцены.");
                return;
            }

            var point = playerSpawnData.SpawnPoint;
            await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.PLAYER, point.position, point.rotation);
        }
    }
}