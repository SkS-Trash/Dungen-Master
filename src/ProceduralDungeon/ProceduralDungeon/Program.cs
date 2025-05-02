namespace ProceduralDungeon
{
    internal static class Program
    {
        private static void Main()
        {
            var width = 50;
            var height = 30;
            var roomCount = 8;
            var roomMinSize = 5;
            var roomMaxSize = 10;
            var cellSize = 20;
            var seed = 0;

            var random = new Random(seed);

            var generator = new DungeonGenerator(
                new MapGenerator(width, height, random),
                new DecorGenerator(width, height, random),
                new EnemySpawner(width, height, random)
            );
            generator.GenerateDungeon(roomCount, roomMinSize, roomMaxSize);

            var renderer = new CompositeImageDungeonRenderer();
            renderer.RenderDungeon(generator, cellSize);
        }
    }
}