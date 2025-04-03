using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Factories.GameObject;
using ProceduralDungeon;
using Providers.Assets;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class ConstructionMapState : IStateOneShot<(TileType[,] map, LevelStyleConfig config)>
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IAssetsProvider _assetsProvider;

        public ConstructionMapState(
            IGameObjectFactory gameObjectFactory,
            IAssetsProvider assetsProvider
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _assetsProvider = assetsProvider;
        }

        public async UniTask OnEnterAsync((TileType[,] map, LevelStyleConfig config) data)
        {
            await ConstructionLayer(data.map, data.config);
        }

        private async UniTask ConstructionLayer(TileType[,] mapLayer, LevelStyleConfig dataConfig)
        {
            var parent = await CreatedParent();

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] == TileType.Empty) continue;

                await InstantCell(mapLayer, dataConfig, x, y, parent);
            }
        }

        private async Task<Transform> CreatedParent()
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "Dungeon map";
            var parent = parentObject.transform;
            return parent;
        }

        private async UniTask InstantCell(TileType[,] mapLayer, LevelStyleConfig dataConfig, int x, int y,
            Transform parent)
        {
            var tileType = mapLayer[x, y];

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