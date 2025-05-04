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
    public class ConstructionMapState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _containerProvider;

        public ConstructionMapState(
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

            await ConstructionLayer(container.MapLayer, container.LevelStyleConfig);
        }

        private async UniTask ConstructionLayer(TileType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            var parent = await CreatedParent();

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == TileType.Empty) continue;

                if (mapLayer[x, y] == TileType.Start || mapLayer[x, y] == TileType.Exit)
                    await InstantCell(TileType.Floor, dataConfig, x, y, parent);

                await InstantCell(mapLayer[x, y], dataConfig, x, y, parent);
            }
        }

        private async Task<Transform> CreatedParent()
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "Dungeon map";
            var parent = parentObject.transform;
            return parent;
        }

        private async UniTask InstantCell(TileType tileType, LevelStyleConfig dataConfig, int x, int y,
            Transform parent)
        {
            var prefabs = dataConfig.GetTileConfig(tileType).Prefabs;

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