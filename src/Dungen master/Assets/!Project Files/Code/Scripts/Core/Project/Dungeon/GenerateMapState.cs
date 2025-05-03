using System;
using Cysharp.Threading.Tasks;
using ProceduralDungeon;
using ProceduralDungeon.Data.Repositories;
using Providers.Containers.Game;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Dungeon
{
    public class GenerateMapState : IStateOneShot
    {
        private readonly IGameContainerProvider _containerProvider;

        public GenerateMapState(
            IGameContainerProvider containerProvider
        )
        {
            _containerProvider = containerProvider;
        }

        public UniTask OnEnterAsync(Unit _)
        {
            var data = _containerProvider.Container;

            var random = new Random(0);
            var mapConfig = MapConfigRepository.GetTestConfig();
            var decorConfig = DecorConfigRepository.GetTestConfig();
            var enemyConfig = EnemyConfigRepository.GetTestConfig();

            var generator = new DungeonGenerator(
                new MapGenerator(mapConfig, random),
                new DecorGenerator(decorConfig, mapConfig.Width, mapConfig.Height, random),
                new EnemySpawner(enemyConfig, mapConfig.Width, mapConfig.Height, random)
            );
            generator.GenerateDungeon();

            data.MapLayer = generator.MapLayer;
            data.DecorLayer = generator.DecorLayer;
            data.EnemyLayer = generator.EnemyLayer;

            return UniTask.CompletedTask;
        }
    }
}