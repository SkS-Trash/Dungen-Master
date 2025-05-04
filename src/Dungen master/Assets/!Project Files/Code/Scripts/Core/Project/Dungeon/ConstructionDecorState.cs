using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Factories.GameObject;
using ProceduralDungeon.Data;
using ProceduralDungeon.Data.Types;
using Providers.Containers.Game;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConstructionDecorState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _containerProvider;

        public ConstructionDecorState(
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

            await ConstructionLayer(container.DecorLayer, container.LevelStyleConfig);
        }

        private async UniTask ConstructionLayer(DecorType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            var parent = await CreatedParent();

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == DecorType.None) continue;

                await InstantCell(mapLayer, dataConfig, x, y, parent);
            }
        }

        private async Task<Transform> CreatedParent()
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "Dungeon decor";
            var parent = parentObject.transform;
            return parent;
        }

        private async UniTask InstantCell(DecorType[,] mapLayer, LevelStyleConfig dataConfig, int x, int y,
            Transform parent)
        {
            var tileType = mapLayer[x, y];

            var prefabs = dataConfig.GetDecorConfig(tileType).Prefabs;

            if (prefabs == null || prefabs.Length == 0) return;

            var assetReference = prefabs[Random.Range(0, prefabs.Length)];

            if (assetReference == null) return;

            await _gameObjectFactory.InstantiateAsync(
                assetReference,
                new Vector3(x, 0, y),
                Quaternion.identity,
                parent
            );
        }
    }
}