using Cysharp.Threading.Tasks;
using Factories.GameObject;
using Providers.Containers;
using Providers.Containers.Scene;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using static GameObjectsPaths;

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

        public async UniTask OnEnterAsync(UnitEmpty _)
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

            var point = playerSpawnData.PlayerSpawnPoint;
            var player = await _gameObjectFactory.InstantiateAsync(PLAYER, point.position, point.rotation);
            playerSpawnData.PlayerTransform = player.transform;
        }
    }
}