using Cysharp.Threading.Tasks;
using Dungeon;
using Infrastructure.Factories.GameObject;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConstructionDecorState : IStateOneShot<(DecorType[,] map, LevelStyleConfig config)>
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public ConstructionDecorState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync((DecorType[,] map, LevelStyleConfig config) data)
        {
            await ConstructionLayer(data.map, data.config);
        }

        private async UniTask ConstructionLayer(DecorType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == DecorType.None) continue;

                await InstantCell(mapLayer, dataConfig, x, y);
            }
        }

        private async UniTask InstantCell(DecorType[,] mapLayer, LevelStyleConfig dataConfig, int x, int y)
        {
            var tileType = mapLayer[x, y];
            var prefabs = dataConfig.GetDecorConfig(tileType).Prefabs;
            var assetReference = prefabs[Random.Range(0, prefabs.Length)];
            await _gameObjectFactory.InstantiateAsync(
                assetReference,
                new Vector3(x, 0, y),
                Quaternion.identity
            );
        }
    }
}