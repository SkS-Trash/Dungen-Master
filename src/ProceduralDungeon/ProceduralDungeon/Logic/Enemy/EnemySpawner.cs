using ProceduralDungeon.Data;
using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon
{
    public class EnemySpawner : IEnemySpawner
    {
        public EnemyType[,] EnemyLayer { get; }

        private readonly Random _random;
        private readonly EnemyConfig _config;

        public EnemySpawner(EnemyConfig config, int width, int height, Random random)
        {
            _config = config;
            EnemyLayer = new EnemyType[width, height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                EnemyLayer[x, y] = EnemyType.None;

            _random = random;
        }

        public void SpawnEnemies(TileType[,] map, DecorType[,] decorLayer, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                var enemyCount = GetEnemyCountForRoom(room);

                for (var i = 0; i < enemyCount; i++)
                {
                    var (x, y) = GetValidEnemyPosition(room, map, decorLayer);
                    if (x == -1 && y == -1) continue;

                    EnemyLayer[x, y] = GetEnemyTypeForRoom(room);
                }
            }
        }

        private int GetEnemyCountForRoom(Room room)
        {
            return room.Type switch
            {
                RoomType.Hard => 1,
                RoomType.Trap => 0,
                _ => _random.Next(0, 3)
            };
        }

        private (int x, int y) GetValidEnemyPosition(Room room, TileType[,] map, DecorType[,] decorLayer)
        {
            for (var attempt = 0; attempt < 20; attempt++)
            {
                var x = _random.Next(room.X + 1, room.X + room.Width - 1);
                var y = _random.Next(room.Y + 1, room.Y + room.Height - 1);

                if (EnemyLayer[x, y] != EnemyType.None) continue;
                if (map[x, y] != TileType.Floor) continue;
                if (decorLayer[x, y] != DecorType.None) continue;

                return (x, y);
            }

            return (-1, -1);
        }

        private EnemyType GetEnemyTypeForRoom(Room room)
        {
            if (room.Type == RoomType.Hard)
                return EnemyType.Boss;
            
            return _random.NextDouble() < 0.5
                ? EnemyType.EnemyIsCloseCombat
                : EnemyType.EnemyRangedCombat;
        }
    }
}