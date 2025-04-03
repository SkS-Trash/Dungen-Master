using Cysharp.Threading.Tasks;
using Factories.GameObject;
using ProceduralDungeon;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class InstantiatePlayerState : IStateOneShot<TileType[,]>
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync(TileType[,] mapLayer)
        {
            var spawnPoint = GetSpawnPoint(mapLayer);

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