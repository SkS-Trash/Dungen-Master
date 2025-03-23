using Cysharp.Threading.Tasks;
using Features.ProceduralDungeon;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Dungeon
{
    public class GenerateMapState : IStateOneShot<DungeonGenerationData>
    {
        public UniTask OnEnterAsync(DungeonGenerationData data)
        {
            var generator = new DungeonGenerator(data.Width, data.Height);

            generator.GenerateDungeon(data.RoomCount, data.RoomMinSize, data.RoomMaxSize);

            data.MapLayer = generator.MapLayer;
            data.DecorLayer = generator.DecorLayer;
            data.EnemyLayer = generator.EnemyLayer;

            return UniTask.CompletedTask;
        }
    }
}