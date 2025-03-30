namespace ProceduralDungeon
{
    public class DungeonGenerator
    {
        public TileType[,] MapLayer { get; private set; }
        public DecorType[,] DecorLayer { get; private set; }
        public EnemyType[,] EnemyLayer { get; private set; }

        private readonly IMapGenerator _mapGenerator;
        private readonly IDecorGenerator _decorGenerator;
        private readonly IEnemySpawner _enemySpawner;
        private readonly Random _random;

        public DungeonGenerator(
            IMapGenerator mapGenerator,
            IDecorGenerator decorGenerator,
            IEnemySpawner enemySpawner,
            int? seed = null
        )
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
            _mapGenerator = mapGenerator;
            _decorGenerator = decorGenerator;
            _enemySpawner = enemySpawner;
        }

        public void GenerateDungeon(int roomCount, int roomMinSize, int roomMaxSize)
        {
            _mapGenerator.GenerateMap(roomCount, roomMinSize, roomMaxSize);
            MapLayer = _mapGenerator.Map;

            _decorGenerator.GenerateDecor(MapLayer, _mapGenerator.Rooms);
            DecorLayer = _decorGenerator.DecorLayer;

            _enemySpawner.SpawnEnemies(MapLayer, _mapGenerator.Rooms);
            EnemyLayer = _enemySpawner.EnemyLayer;
        }
    }
}