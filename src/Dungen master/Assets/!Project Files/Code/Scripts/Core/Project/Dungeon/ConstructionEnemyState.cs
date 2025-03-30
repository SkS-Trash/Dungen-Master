using Cysharp.Threading.Tasks;
using Factories.GameObject;
using ProceduralDungeon;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConstructionEnemyState : IStateOneShot<(EnemyType[,] map, LevelStyleConfig config)>
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public ConstructionEnemyState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync((EnemyType[,] map, LevelStyleConfig config) data)
        {
            await ConstructionLayer(data.map, data.config);
        }

        private async UniTask ConstructionLayer(EnemyType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == EnemyType.None) continue;

                await InstantCell(mapLayer, dataConfig, x, y);
            }
        }

        private async UniTask InstantCell(EnemyType[,] mapLayer, LevelStyleConfig dataConfig, int x, int y)
        {
            var tileType = mapLayer[x, y];
            
            var prefabs = dataConfig.GetEnemyConfig(tileType).Prefabs;
            
            var assetReference = prefabs[Random.Range(0, prefabs.Length)];

            if (assetReference == null) return;
            
            await _gameObjectFactory.InstantiateAsync(
                assetReference,
                new Vector3(x, 0, y),
                Quaternion.identity
            );
        }
    }
}