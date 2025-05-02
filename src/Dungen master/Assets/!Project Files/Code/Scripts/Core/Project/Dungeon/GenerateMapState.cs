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

        public UniTask OnEnterAsync(Unit _)
        {
            var data = _containerProvider.Container;

            var generator = new DungeonGenerator(
                new MapGenerator(data.Width, data.Height, data.Seed),
                new DecorGenerator(data.Width, data.Height, data.Seed),
                new EnemySpawner(data.Width, data.Height, data.Seed)
            );

            generator.GenerateDungeon(data.RoomCount, data.RoomMinSize, data.RoomMaxSize);

            data.MapLayer = generator.MapLayer;
            data.DecorLayer = generator.DecorLayer;
            data.EnemyLayer = generator.EnemyLayer;

            return UniTask.CompletedTask;
        }
    }
}