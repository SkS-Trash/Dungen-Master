using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Enemy;
using Factories.GameObject;
using ProceduralDungeon;
using Providers.Containers.Game;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConstructionEnemyState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _containerProvider;

        private Transform _playerTransform;

        public ConstructionEnemyState(
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
            _playerTransform = container.PlayerTransform;

            await ConstructionLayer(container.EnemyLayer, container.LevelStyleConfig);
        }

        private async UniTask ConstructionLayer(EnemyType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            var parent = await CreatedParent();

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == EnemyType.None) continue;

                await InstantCell(mapLayer[x, y], dataConfig, x, y, parent);
            }
        }

        private async Task<Transform> CreatedParent()
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "Dungeon enemies";
            var parent = parentObject.transform;
            return parent;
        }

        private async UniTask InstantCell(EnemyType enemyType, LevelStyleConfig dataConfig, int x, int y,
            Transform parent)
        {
            var prefabs = dataConfig.GetEnemyConfig(enemyType).Prefabs;

            if (prefabs == null || prefabs.Length == 0) return;

            var assetReference = prefabs[Random.Range(0, prefabs.Length)];

            if (assetReference == null) return;

            var enemyInstance = await _gameObjectFactory.InstantiateAsync(
                assetReference,
                new Vector3(x, 0, y),
                Quaternion.identity,
                parent
            );

            enemyInstance.GetComponent<EnemyCore>()
                .SetPlayerTransform(_playerTransform)
                .Initialize();
        }
    }
}