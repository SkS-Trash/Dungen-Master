using Cysharp.Threading.Tasks;
using Factories.GameObject;
using Player;
using ProceduralDungeon;
using Providers.Containers.Game;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using static GameObjectsPaths;

namespace Core.Project.Dungeon
{
    public class InstantiatePlayerState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _gameContainer;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory,
            IGameContainerProvider gameContainer
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _gameContainer = gameContainer;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var container = _gameContainer.Container;

            FindAndSetupSpawnPoint(container);

            await InstantiatePlayer(container);
        }

        private void FindAndSetupSpawnPoint(IGameContainer container)
        {
            var mapLayer = container.MapLayer;

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] != TileType.Start)
                    continue;

                var playerSpawnPoint = new GameObject("PlayerSpawnPoint")
                {
                    transform =
                    {
                        position = new Vector3(x, 0, y)
                    }
                };

                container.PlayerSpawnPoint = playerSpawnPoint.transform;

                return;
            }

            container.PlayerSpawnPoint = null;
        }

        private async UniTask InstantiatePlayer(IGameContainer container)
        {
            var spawnPoint = container.PlayerSpawnPoint.position;

            var player = await _gameObjectFactory.InstantiateAsync(
                PLAYER,
                spawnPoint,
                Quaternion.identity
            );

            container.PlayerTransform = player.GetComponentInChildren<ThirdPersonController>().transform;
        }
    }
}