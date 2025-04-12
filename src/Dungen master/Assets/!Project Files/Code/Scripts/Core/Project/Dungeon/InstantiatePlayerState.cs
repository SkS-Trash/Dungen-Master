using Cysharp.Threading.Tasks;
using Factories.GameObject;
using ProceduralDungeon;
using Providers.Containers.Game;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class InstantiatePlayerState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _containerProvider;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory,
            IGameContainerProvider containerProvider
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _containerProvider = containerProvider;
        }


        public async UniTask OnEnterAsync(Unit _)
        {
            var container = _containerProvider.Container;

            var spawnPoint = GetSpawnPoint(container.MapLayer);

            await InstantiatePlayerUI(spawnPoint);
        }

        private Vector2Int GetSpawnPoint(TileType[,] mapLayer)
        {
            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == TileType.Start)
                {
                    return new Vector2Int(x, y);
                }
            }

            return Vector2Int.zero;
        }

        private async UniTask InstantiatePlayerUI(Vector2Int spawnPoint)
        {
            await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.PLAYER,
                new Vector3(spawnPoint.x, 0, spawnPoint.y), Quaternion.identity);
        }
    }
}