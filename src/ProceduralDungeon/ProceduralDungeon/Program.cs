using ProceduralDungeon.Data.Repositories;

namespace ProceduralDungeon
{
    internal static class Program
    {
        private static void Main()
        {
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

            var cellSize = 20;
            var renderer = new CompositeImageDungeonRenderer();
            renderer.RenderDungeon(generator, cellSize);

            // var renderer = new ConsoleDungeonRenderer();
            // renderer.RenderDungeon(generator, cellSize);
            // Console.WriteLine("\n\t---\tPress any key to exit...\t---\t");
            // Console.ReadKey();
        }
    }
}