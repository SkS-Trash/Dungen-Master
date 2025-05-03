namespace ProceduralDungeon
{
    public class EnemySpawner : IEnemySpawner
    {
        public EnemyType[,] EnemyLayer { get; }

        private readonly Random _random;

        public EnemySpawner(int width, int height, Random random)
        {
            EnemyLayer = new EnemyType[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                EnemyLayer[x, y] = EnemyType.None;
            }

            _random = random;
        }

        public void SpawnEnemies(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                var enemyCount = room.Type switch
                {
                    RoomType.Hard => 1,
                    RoomType.Trap => 0,
                    _ => _random.Next(0, 3)
                };

                for (var i = 0; i < enemyCount; i++)
                {
                    var x = _random.Next(room.X + 1, room.X + room.Width - 1);
                    var y = _random.Next(room.Y + 1, room.Y + room.Height - 1);

                    if (EnemyLayer[x, y] != EnemyType.None) continue;

                    if (map[x, y] != TileType.Floor) continue;

                    EnemyLayer[x, y] = room.Type == RoomType.Hard
                        ? EnemyType.Boss
                        : _random.NextDouble() < 0.5
                            ? EnemyType.EnemyIsCloseCombat
                            : EnemyType.EnemyRangedCombat;
                }
            }
        }
    }
}