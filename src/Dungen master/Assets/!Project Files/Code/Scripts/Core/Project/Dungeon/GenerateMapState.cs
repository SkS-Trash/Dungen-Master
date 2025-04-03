using Cysharp.Threading.Tasks;
using ProceduralDungeon;
using ProceduralDungeon.Decor;
using ProceduralDungeon.Enemy;
using ProceduralDungeon.Map;
using StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Dungeon
{
    public class GenerateMapState : IStateOneShot<DungeonGenerationData>
    {
        public UniTask OnEnterAsync(DungeonGenerationData data)
        {
            var generator = new DungeonGenerator(
                new MapGenerator(data.Width, data.Height),
                new DecorGenerator(data.Width, data.Height),
                new EnemySpawner(data.Width, data.Height)
            );

            generator.GenerateDungeon(data.RoomCount, data.RoomMinSize, data.RoomMaxSize);

            data.MapLayer = generator.MapLayer;
            data.DecorLayer = generator.DecorLayer;
            data.EnemyLayer = generator.EnemyLayer;

            return UniTask.CompletedTask;
        }
    }
}