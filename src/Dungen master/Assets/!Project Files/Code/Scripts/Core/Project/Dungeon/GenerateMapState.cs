using System;
using Cysharp.Threading.Tasks;
using ProceduralDungeon;
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

        public UniTask OnEnterAsync(UnitEmpty _)
        {
            var data = _containerProvider.Container;
            var random = new Random(data.Seed);
            var generator = new DungeonGenerator(
                new MapGenerator(data.MapGeneratorConfig, random),
                new DecorGenerator(data.DecorConfig, data.MapGeneratorConfig.Width, data.MapGeneratorConfig.Height, random),
                new EnemySpawner(data.EnemyConfig, data.MapGeneratorConfig.Width, data.MapGeneratorConfig.Height, random)
            );

            generator.GenerateDungeon();

            data.MapLayer = generator.MapLayer;
            data.DecorLayer = generator.DecorLayer;
            data.EnemyLayer = generator.EnemyLayer;

            return UniTask.CompletedTask;
        }
    }
}